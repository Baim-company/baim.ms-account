using PersonalAccount.API.Models.Entities.Agiles.Projects;
using Global.Infrastructure.Exceptions.PersonalAccount;
using PersonalAccount.API.Models.Dtos.Agiles.Projects;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Services.Abstractions;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Models.Enums;
using PersonalAccount.API.Models.Dtos;
using Microsoft.EntityFrameworkCore;


namespace PersonalAccount.API.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly AgileDbContext _agileDbContext;
    public ProjectService(AgileDbContext agileDbContext)
    {
        _agileDbContext = agileDbContext;
    }


    public async Task<PagedResponse<Project>> GetProjcetsByCompanyAsync(Guid companyId,
        PaginationParameters paginationParameters,
        string? onSearch = null,
        string? onProduct = null,
        bool sortByCreationDate = false)
    {
        var projectsQuery = _agileDbContext.Projects
            .Include(p => p.Company)
            .Include(p => p.Product)
            .Include(p => p.ProjectUsers).ThenInclude(p => p.Staff).ThenInclude(m => m.User)
            .Include(p => p.ProjectUsers).ThenInclude(p => p.Client).ThenInclude(m => m.User)
            .Include(p => p.ProjectManager).ThenInclude(m => m.User)
            .Where(p => p.CompanyId == companyId && p.IsPublic)
            .AsNoTracking()
            .AsQueryable();


        if (!string.IsNullOrEmpty(onSearch))
        {
            projectsQuery = projectsQuery.Where(project =>
                (project.Company!.CompanyName != null && project.Company!.CompanyName.ToLower().Contains(onSearch.ToLower())) ||
                (project.Name != null && project.Name.ToLower().Contains(onSearch.ToLower())) ||
                (project.Product != null && project.Product.Name.ToLower().Contains(onSearch.ToLower()) ||
                (project.ProjectManager!.User != null && project.ProjectManager.User!.Name.ToLower().Contains(onSearch.ToLower())))
            );
        }
        if (!string.IsNullOrEmpty(onProduct))
        {
            projectsQuery = projectsQuery.Where(project =>
                project.Product != null && project.Product.Name.ToLower().Contains(onProduct.ToLower())
            );
        }

        // Фильтр по дате создания
        projectsQuery = sortByCreationDate
            ? projectsQuery.OrderBy(project => project.CreatedDate)
            : projectsQuery.OrderByDescending(project => project.CreatedDate);


        var totalRecords = await projectsQuery.CountAsync();
        var paginatedProjects = await projectsQuery
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();

        return new PagedResponse<Project>(paginatedProjects, paginationParameters.PageNumber, paginationParameters.PageSize, totalRecords);
    }



    public async Task<PagedResponse<Project>> GetProjcetsByParticipationAsync(Guid participationId,
        PaginationParameters paginationParameters,
        string? onSearch = null,
        string? onProduct = null,
        bool sortByCreationDate = false)
    {
        var projectsQuery = _agileDbContext.Projects
            .Include(p => p.Company)
            .Include(p => p.Product)
            .Include(p => p.ProjectUsers).ThenInclude(p => p.Staff).ThenInclude(m => m.User)
            .Include(p => p.ProjectUsers).ThenInclude(p => p.Client).ThenInclude(m => m.User)
            .Include(p => p.ProjectManager).ThenInclude(m => m.User)
            .AsNoTracking()
            .AsQueryable();


        // Фильтр по участию в проекте
        projectsQuery = projectsQuery.Where(p =>
            p.ProjectUsers.Any(pu => pu.ClientId == participationId || pu.StaffId == participationId));


        if (!string.IsNullOrEmpty(onSearch))
        {
            projectsQuery = projectsQuery.Where(project =>
                (project.Company!.CompanyName != null && project.Company!.CompanyName.ToLower().Contains(onSearch.ToLower())) ||
                (project.Name != null && project.Name.ToLower().Contains(onSearch.ToLower())) ||
                (project.Product != null && project.Product.Name.ToLower().Contains(onSearch.ToLower()) ||
                (project.ProjectManager!.User != null && project.ProjectManager.User!.Name.ToLower().Contains(onSearch.ToLower())))
            );
        }
        if (!string.IsNullOrEmpty(onProduct))
        {
            projectsQuery = projectsQuery.Where(project =>
                project.Product != null && project.Product.Name.ToLower().Contains(onProduct.ToLower())
            );
        }

        // Фильтр по дате создания
        projectsQuery = sortByCreationDate
            ? projectsQuery.OrderBy(project => project.CreatedDate)
            : projectsQuery.OrderByDescending(project => project.CreatedDate);


        var totalRecords = await projectsQuery.CountAsync();
        var paginatedProjects = await projectsQuery
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();

        return new PagedResponse<Project>(paginatedProjects, paginationParameters.PageNumber, paginationParameters.PageSize, totalRecords);
    }




    public async Task<PagedResponse<Project>> GetProjcetsAsync(PaginationParameters paginationParameters,
        string? onSearch = null,
        string? onProduct = null,
        bool sortByCreationDate = false)
    {
        var projectsQuery = _agileDbContext.Projects
            .Include(p => p.Company)
            .Include(p => p.Product)
            .Include(p => p.ProjectUsers).ThenInclude(p => p.Staff).ThenInclude(m => m.User)
            .Include(p => p.ProjectUsers).ThenInclude(p => p.Client).ThenInclude(m => m.User)
            .Include(p => p.ProjectManager).ThenInclude(m => m.User)
            .AsNoTracking()
            .AsQueryable();


        if (!string.IsNullOrEmpty(onSearch))
        {
            projectsQuery = projectsQuery.Where(project =>
                (project.Company!.CompanyName != null && project.Company!.CompanyName.ToLower().Contains(onSearch.ToLower())) ||
                (project.Name != null && project.Name.ToLower().Contains(onSearch.ToLower())) ||
                (project.Product != null && project.Product.Name.ToLower().Contains(onSearch.ToLower()) ||
                (project.ProjectManager!.User != null && project.ProjectManager.User!.Name.ToLower().Contains(onSearch.ToLower())))
            );
        }
        if (!string.IsNullOrEmpty(onProduct))
        {
            projectsQuery = projectsQuery.Where(project =>
                project.Product != null && project.Product.Name.ToLower().Contains(onProduct.ToLower())
            );
        }
        // Фильтр по дате создания
        projectsQuery = sortByCreationDate
            ? projectsQuery.OrderBy(project => project.CreatedDate)
            : projectsQuery.OrderByDescending(project => project.CreatedDate);


        var totalRecords = await projectsQuery.CountAsync();
        var paginatedProjects = await projectsQuery
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();

        return new PagedResponse<Project>(paginatedProjects, paginationParameters.PageNumber, paginationParameters.PageSize, totalRecords);
    }




    public async Task<Response<Project>> GetProjectAsync(Guid id)
    {
        var projectExist = await _agileDbContext.Projects
            .Include(p => p.Company)
            .Include(p => p.Product)
            .Include(p => p.ProjectUsers).ThenInclude(p => p.Staff).ThenInclude(m => m.User)
            .Include(p => p.ProjectUsers).ThenInclude(p => p.Client).ThenInclude(m => m.User)
            .Include(p => p.ProjectManager).ThenInclude(m => m.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && p.IsPublic);

        if (projectExist == null) throw new PersonalAccountException(PersonalAccountErrorType.ProjectNotFound, $"Error!Failed to get project with id: {id}");

        return new Response<Project>("Success", projectExist);
    }





    public async Task<Response<Project>> AddProjectAsync(ProjectModel projectModel)
    {
        var productExist = await _agileDbContext.Products.FindAsync(projectModel.ProductId);
        if (productExist == null) throw new PersonalAccountException(PersonalAccountErrorType.ProductNotFound, $"Error!Product with product id: {projectModel.ProductId} doesn't exist!");


        var managerExist = await _agileDbContext.Staff.FindAsync(projectModel.ManagerId);
        if (managerExist == null) throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Error!Staff with manager id: {projectModel.ManagerId} doesn't exist!");


        var projectExist = await _agileDbContext.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == projectModel.Name);
        if (projectExist != null) throw new PersonalAccountException(PersonalAccountErrorType.ProjectAlreadyExists, "Error!Failed to create project!\nProject with this name already exist!");


        Project project = new Project(projectModel);
        await _agileDbContext.Projects.AddAsync(project);


        if (projectModel.ProjectStaffs != null || projectModel.ProjectClients != null)
        {
            List<ProjectUser> users = new List<ProjectUser>();
            if (projectModel.ProjectStaffs != null)
            {
                foreach (var staffId in projectModel.ProjectStaffs)
                {
                    var staff = await _agileDbContext.Staff.FindAsync(staffId);
                    if (staff == null) throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Error!Staff with staff id: {staffId} doesn't exist!");

                    var prrojectUser = new ProjectUser() { Staff = staff, StaffId = staff.Id, Project = project, ProjectId = project.Id };
                    users.Add(prrojectUser);
                }
            }
            if (projectModel.ProjectClients != null && projectModel.ProjectType == ProjectType.External)
            {
                foreach (var clientId in projectModel.ProjectClients)
                {
                    var client = await _agileDbContext.Clients.FindAsync(clientId);
                    if (client == null) throw new PersonalAccountException(PersonalAccountErrorType.ClientNotFound, $"Error!Client with client id: {clientId} doesn't exist!");

                    var prrojectUser = new ProjectUser() { Client = client, ClientId = client.Id, Project = project, ProjectId = project.Id };
                    users.Add(prrojectUser);
                }
            }
            await _agileDbContext.ProjectUsers.AddRangeAsync(users);
        }
        await _agileDbContext.SaveChangesAsync();


        return new Response<Project>("Successfully created!", project);
    }





    public async Task<Response<Project>> CompleteProjectAsync(Guid id)
    {
        var projectExist = await _agileDbContext.Projects.FindAsync(id);
        if (projectExist == null) throw new PersonalAccountException(PersonalAccountErrorType.ProjectNotFound,
            $"Error!Failed to get project!\nProject with id: {id} not found!");

        await _agileDbContext.Projects
        .Where(p => p.Id == id)
        .ExecuteUpdateAsync(p => p
            .SetProperty(p => p.IsCompleted, !projectExist.IsCompleted));
        await _agileDbContext.SaveChangesAsync();

        return new Response<Project>("Successfully updated!", new Project());
    }





    public async Task<Response<Project>> MakeProjectPublicAsync(Guid id)
    {
        var projectExist = await _agileDbContext.Projects.FindAsync(id);
        if (projectExist == null) throw new PersonalAccountException(PersonalAccountErrorType.ProjectNotFound,
            $"Error!Failed to get project!\nProject with id: {id} doesn't exist!");

        await _agileDbContext.Projects
        .Where(p => p.Id == id)
        .ExecuteUpdateAsync(p => p
            .SetProperty(p => p.IsPublic, !projectExist.IsPublic));
        await _agileDbContext.SaveChangesAsync();

        return new Response<Project>("Successfully updated!", new Project());
    }



    public async Task<Response<Project>> DeleteProjectAsync(Guid id)
    {
        var projectExist = await _agileDbContext.Projects.FindAsync(id);
        if (projectExist == null) throw new PersonalAccountException(PersonalAccountErrorType.ProjectNotFound, $"Error!Failed to find project with id: {id}");

        await _agileDbContext.Projects
            .Where(p => p.Id == id).ExecuteDeleteAsync();
        await _agileDbContext.SaveChangesAsync();

        return new Response<Project>("Successfully deleted!", new Project());
    }



    public async Task<Response<Project>> UpdateProjectAsync(UpdateProjectModel projectModel)
    {
        var productExist = await _agileDbContext.Products.FindAsync(projectModel.ProductId);
        if (productExist == null) throw new PersonalAccountException(PersonalAccountErrorType.ProductNotFound, $"Error!Product with product id: {projectModel.ProductId} doesn't exist!");

        var staffExist = await _agileDbContext.Staff.FindAsync(projectModel.ManagerId);
        if (staffExist == null) throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Error!Staff with manager id: {projectModel.ManagerId} doesn't exist!");

        var projectExist = await _agileDbContext.Projects
            .Include(p => p.ProjectManager)
            .Include(p => p.Product)
            .FirstOrDefaultAsync(p => p.Id == projectModel.Id);
        if (projectExist == null) throw new PersonalAccountException(PersonalAccountErrorType.ProjectNotFound, $"Error!Failed to get project!\nProject with this id: {projectModel.Id} doen't exist!");


        var result = await _updateProjectUsers(projectModel, projectExist);
        if (result.Data == null) return new Response<Project>(result.Message, null);




        projectExist.ProductId = productExist.Id;
        projectExist.ProjectManagerId = staffExist.Id;

        projectExist.Name = projectModel.Name;
        projectExist.Description = projectModel.Description;
        projectExist.DesignThemeImagePath = projectModel.DesignThemeImagePath;
        projectExist.ProjectAvatarImagePath = projectModel.ProjectAvatarImagePath;

        _agileDbContext.Projects.Update(projectExist);
        await _agileDbContext.SaveChangesAsync();

        return new Response<Project>("Successfully updated!", projectExist);
    }

    private async Task<Response<Project>> _updateProjectUsers(UpdateProjectModel projectModel, Project project)
    {
        var projectUsers = await _agileDbContext.ProjectUsers
            .AsNoTracking()
            .Where(pu => pu.ProjectId == projectModel.Id)
            .ToListAsync();

        List<ProjectUser> users = new List<ProjectUser>();

        if (projectModel.ProjectStaffs != null || projectModel.ProjectClients != null)
        {
            if (projectModel.ProjectStaffs != null)
            {
                var staffUsersToRemove = projectUsers.Where(pu => pu.StaffId.HasValue).ToList();
                _agileDbContext.ProjectUsers.RemoveRange(staffUsersToRemove);

                foreach (var staffId in projectModel.ProjectStaffs)
                {
                    var staff = await _agileDbContext.Staff.FindAsync(staffId);
                    if (staff == null) throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Error!Staff with staff id: {staffId} doesn't exist!");

                    var projectUser = new ProjectUser() { Staff = staff, StaffId = staff.Id, Project = project, ProjectId = project.Id };
                    users.Add(projectUser);
                }
            }

            if (projectModel.ProjectClients != null && project.ProjectType == ProjectType.External)
            {
                var clientUsersToRemove = projectUsers.Where(pu => pu.ClientId.HasValue).ToList();
                _agileDbContext.ProjectUsers.RemoveRange(clientUsersToRemove);

                foreach (var clientId in projectModel.ProjectClients)
                {
                    var client = await _agileDbContext.Clients.FindAsync(clientId);
                    if (client == null) throw new PersonalAccountException(PersonalAccountErrorType.ClientNotFound, $"Error!Client with client id: {clientId} doesn't exist!");

                    var projectUser = new ProjectUser() { Client = client, ClientId = client.Id, Project = project, ProjectId = project.Id };
                    users.Add(projectUser);
                }
            }

            await _agileDbContext.SaveChangesAsync();

            await _agileDbContext.ProjectUsers.AddRangeAsync(users);
            await _agileDbContext.SaveChangesAsync();
        }

        return new Response<Project>("Success", project);
    }
}