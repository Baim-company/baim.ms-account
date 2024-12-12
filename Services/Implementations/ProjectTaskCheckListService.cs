using AutoMapper;
using Microsoft.EntityFrameworkCore; 
using PersonalAccount.API.Models.Dtos.Agiles.CheckItems;
using PersonalAccount.API.Models.Entities.Agiles.CheckItems;
using PersonalAccount.API.Models.Entities.Agiles.CheckLists;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Dtos.Responses; 
using PersonalAccount.API.Data.DbContexts;
using Global.Infrastructure.Exceptions.PersonalAccount;

namespace PersonalAccount.API.Services.Implementations;

public class ProjectTaskCheckListService : IProjectTaskCheckListService
{

    private readonly AgileDbContext _context;
    private readonly IMapper _mapper;

    public ProjectTaskCheckListService(AgileDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }



    public async Task<Response<ProjectTaskCheckListModel>> GetCheckListByTaskIdAsync(Guid projectTaskId)
    {
        try
        {
            var checkList = await _context.ProjectTaskCheckLists
                .Where(cl => cl.ProjectTaskId == projectTaskId)
                .Select(cl => new ProjectTaskCheckListModel
                {
                    Id = cl.Id,
                    Title = cl.Title,
                    CreatedDate = cl.CreatedDate,
                    IsImportant = cl.IsImportant,
                    ProjectTaskId = cl.ProjectTaskId,
                    ProjectTaskCheckItems = cl.ProjectTaskCheckItems.Select(ci => new ProjectTaskCheckItemModel
                    {
                        Id = ci.Id,
                        Name = ci.Name,
                        IsDone = ci.IsDone,
                        IsImportant = ci.IsImportant,
                        ProjectTaskCheckItemFiles = ci.ProjectTaskCheckItemFiles.Select(f => new ProjectTaskCheckItemFileModel
                        {
                            Id = f.Id,
                            FileName = f.FileName,
                            FileExtension = f.FileExtension,
                            FileContentBase64 = Convert.ToBase64String(f.FileData)
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (checkList == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, "CheckList not found for the specified task.");

            return new Response<ProjectTaskCheckListModel>("CheckList retrieved successfully", checkList);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<ProjectTaskCheckListModel>> UpdateCheckListAsync(ProjectTaskCheckListModel checkListModel)
    {
        try
        {
            var existingCheckList = await _context.ProjectTaskCheckLists
                .Include(cl => cl.ProjectTaskCheckItems)
                .ThenInclude(ci => ci.ProjectTaskCheckItemFiles)
                .FirstOrDefaultAsync(cl => cl.Id == checkListModel.Id);

            if (existingCheckList == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, "CheckList not found.");

            UpdateExistingCheckListAsync(existingCheckList, checkListModel);
            await _context.SaveChangesAsync();

            var updatedCheckListModel = _mapper.Map<ProjectTaskCheckListModel>(existingCheckList);
            return new Response<ProjectTaskCheckListModel>("CheckList updated successfully", updatedCheckListModel);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<Response<bool>> DeleteCheckListAsync(Guid id)
    {
        try
        {
            var checkList = await _context.ProjectTaskCheckLists
                .Include(cl => cl.ProjectTaskCheckItems)
                .ThenInclude(ci => ci.ProjectTaskCheckItemFiles)
                .FirstOrDefaultAsync(cl => cl.Id == id);

            if (checkList == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, "CheckList not found.");

            _context.ProjectTaskCheckLists.Remove(checkList);
            await _context.SaveChangesAsync();

            return new Response<bool>("CheckList deleted successfully", true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    private async Task<ProjectTaskCheckList> CreateCheckListAsync(ProjectTaskCheckListCreateModel checkListModel, ProjectTask projectTask)
    {
        var checkList = _mapper.Map<ProjectTaskCheckList>(checkListModel);
        checkList.ProjectTaskId = projectTask.Id;

        _context.ProjectTaskCheckLists.Add(checkList);
        await _context.SaveChangesAsync();

        if (checkListModel.ProjectTaskCheckItems != null)
        {
            foreach (var itemModel in checkListModel.ProjectTaskCheckItems)
            {
                var newItem = _mapper.Map<ProjectTaskCheckItem>(itemModel);
                newItem.CheckListId = checkList.Id; 
                _context.ProjectTaskCheckItems.Add(newItem);

                if (itemModel.ProjectTaskCheckItemFiles != null)
                {
                    foreach (var fileModel in itemModel.ProjectTaskCheckItemFiles)
                    {
                        var newFile = _mapper.Map<ProjectTaskCheckItemFile>(fileModel);
                        newFile.ProjectTaskCheckItemId = newItem.Id; 
                        newFile.FileData = Convert.FromBase64String(fileModel.FileContentBase64);
                        newFile.FileExtension = fileModel.FileExtension;
                        _context.ProjectTaskCheckItemFiles.Add(newFile);
                    }
                }
            }
        }

        await _context.SaveChangesAsync();
        return checkList;
    }


    public async Task<Response<ProjectTaskCheckListModel>> CreateCheckListAsync(ProjectTaskCheckListCreateModel checkListCreateModel)
    {
        try
        {
            var projectTask = await _context.ProjectTasks.FindAsync(checkListCreateModel.ProjectTaskId);
            if (projectTask == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, "ProjectTask not found.");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var checkList = await CreateCheckListAsync(checkListCreateModel, projectTask);
                    await transaction.CommitAsync();

                    var checkListModelResult = _mapper.Map<ProjectTaskCheckListModel>(checkList);
                    return new Response<ProjectTaskCheckListModel>("CheckList created successfully", checkListModelResult);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task UpdateExistingCheckListAsync(ProjectTaskCheckList checkList, ProjectTaskCheckListModel checkListModel)
    {
        _mapper.Map(checkListModel, checkList);

        if (checkListModel.ProjectTaskCheckItems != null)
        {
            foreach (var itemModel in checkListModel.ProjectTaskCheckItems)
            {
                var existingItem = await _context.ProjectTaskCheckItems
                                                 .FirstOrDefaultAsync(ci => ci.Id == itemModel.Id);

                if (existingItem == null)
                {
                    var newItem = _mapper.Map<ProjectTaskCheckItem>(itemModel);
                    newItem.CheckListId = checkList.Id;
                    _context.ProjectTaskCheckItems.Add(newItem);

                    if (itemModel.ProjectTaskCheckItemFiles != null)
                    {
                        await AddOrUpdateFilesAsync(newItem, itemModel.ProjectTaskCheckItemFiles);
                    }
                }
                else
                {
                    _mapper.Map(itemModel, existingItem);
                    await UpdateCheckItemFilesAsync(existingItem, itemModel.ProjectTaskCheckItemFiles);
                }
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task AddOrUpdateFilesAsync(ProjectTaskCheckItem newItem, IEnumerable<ProjectTaskCheckItemFileModel> fileModels)
    {
        foreach (var fileModel in fileModels)
        {
            var newFile = _mapper.Map<ProjectTaskCheckItemFile>(fileModel);
            newFile.ProjectTaskCheckItemId = newItem.Id;
            newFile.FileData = Convert.FromBase64String(fileModel.FileContentBase64);
            newFile.FileExtension = fileModel.FileExtension;

            _context.ProjectTaskCheckItemFiles.Add(newFile);
        }
    }

    private async Task UpdateCheckItemFilesAsync(ProjectTaskCheckItem existingItem, IEnumerable<ProjectTaskCheckItemFileModel> fileModels)
    {
        if (fileModels != null)
        {
            foreach (var fileModel in fileModels)
            {
                var existingFile = existingItem.ProjectTaskCheckItemFiles
                                                .FirstOrDefault(f => f.Id == fileModel.Id);

                if (existingFile == null)
                {
                    var newFile = _mapper.Map<ProjectTaskCheckItemFile>(fileModel);
                    newFile.ProjectTaskCheckItemId = existingItem.Id;
                    newFile.FileData = Convert.FromBase64String(fileModel.FileContentBase64);
                    newFile.FileExtension = fileModel.FileExtension;

                    existingItem.ProjectTaskCheckItemFiles.Add(newFile);
                }
                else
                {
                    _mapper.Map(fileModel, existingFile);
                    existingFile.FileData = Convert.FromBase64String(fileModel.FileContentBase64);
                    existingFile.FileExtension = fileModel.FileExtension;
                }
            }
        }
    }
}