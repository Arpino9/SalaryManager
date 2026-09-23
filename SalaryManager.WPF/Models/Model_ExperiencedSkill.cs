using DocumentFormat.OpenXml.Bibliography;
using System.Data.Entity;

namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - 保有スキル
/// </summary>
public class Model_ExperiencedSkill : ModelBase<ViewModel_ExperiencedSkill>, IViewableMaster
{
    #region Get Instance

    private static Model_ExperiencedSkill model = null;

    public static Model_ExperiencedSkill GetInstance(IProjectRepository repository)
    {
        if (model == null)
        {
            model = new Model_ExperiencedSkill(repository);
        }

        return model;
    }

    #endregion

    /// <summary> Repository </summary>
    private IProjectRepository _repository;

    public Model_ExperiencedSkill(IProjectRepository repository)
    {
        _repository = repository;
    }

    /// <summary> ViewModel - 保有スキル </summary>
    internal override ViewModel_ExperiencedSkill ViewModel { get; set; }

    public void Clear_InputForm()
    {
        throw new NotImplementedException();
    }

    public void Initialize()
    {
        Projects.Create(_repository);

        this.ViewModel.ExperiencedLanguages_ItemSource.Clear();

        foreach (var language in this.GetExperienceLanguageYears())
        {
            if (language.Year <= 0)
            {
                this.ViewModel.ExperiencedLanguages_ItemSource.Add(new ExperiencedLanguages(language.Language, "1年未満"));
            }
            else
            {
                this.ViewModel.ExperiencedLanguages_ItemSource.Add(new ExperiencedLanguages(language.Language, $"{language.Year}年以上"));
            }
        }

        this.ViewModel.ExperiencedDatabases_ItemSource.Clear();

        foreach (var database in this.GetExperienceDatabaseYears())
        {
            if (database.Year <= 0)
            {
                this.ViewModel.ExperiencedDatabases_ItemSource.Add(new ExperiencedDatabases(database.Database, "1年未満"));
            }
            else
            {
                this.ViewModel.ExperiencedDatabases_ItemSource.Add(new ExperiencedDatabases(database.Database, $"{database.Year}年以上"));
            }
        }

        this.ViewModel.ExperiencedDevelopmentSupportingTools_ItemSource.Clear();

        foreach (var tool in this.GetExperienceDevelopmentSupportingToolYears())
        {
            if (tool.Year <= 0)
            {
                this.ViewModel.ExperiencedDevelopmentSupportingTools_ItemSource.Add(new ExperiencedDevelopmentSupportingTools(tool.Tool, "1年未満"));
            }
            else
            {
                this.ViewModel.ExperiencedDevelopmentSupportingTools_ItemSource.Add(new ExperiencedDevelopmentSupportingTools(tool.Tool, $"{tool.Year}年以上"));
            }
        }

        this.ViewModel.ExperiencedOtherTools_ItemSource.Clear();

        foreach (var tool in this.GetExperienceOtherToolYears())
        {
            if (tool.Year <= 0)
            {
                this.ViewModel.ExperiencedOtherTools_ItemSource.Add(new ExperiencedOtherTools(tool.Tool, "1年未満"));
            }
            else
            {
                this.ViewModel.ExperiencedOtherTools_ItemSource.Add(new ExperiencedOtherTools(tool.Tool, $"{tool.Year}年以上"));
            }
        }
    }

    public IReadOnlyList<(string Language, double Year)> GetExperienceLanguageYears()
    {
        var languages = Projects.FetchExperienceLanguages();

        var langYears = new List<(string Language, double Year)>();

        foreach (var language in languages)
        {
            if (language == "-" || language == string.Empty)
            {
                continue;
            }

            var projects = Projects.FetchProjectsWithExperienceLanguage(language);
            var years = Math.Round((this.FetchExperienceDays(projects) / 365.0), MidpointRounding.AwayFromZero);
            langYears.Add((language, years));
        }

        return langYears.OrderByDescending(x => x.Year).ToList();
    }

    public IReadOnlyList<(string Database, double Year)> GetExperienceDatabaseYears()
    {
        var databases = Projects.FetchExperienceDatabases();

        var dbYears = new List<(string Database, double Year)>();

        foreach (var database in databases)
        {
            if (database == "-" || database == string.Empty)
            {
                continue;
            }

            var projects = Projects.FetchProjectsWithExperienceDatabase(database);
            var years = Math.Round((this.FetchExperienceDays(projects) / 365.0), MidpointRounding.AwayFromZero);
            dbYears.Add((database, years));
        }

        return dbYears.OrderByDescending(x => x.Year).ToList();
    }

    public IReadOnlyList<(string Tool, double Year)> GetExperienceDevelopmentSupportingToolYears()
    {
        var tools = Projects.FetchExperienceDevelopmentSupportingTools();

        var toolYears = new List<(string Tool, double Year)>();

        foreach (var tool in tools)
        {
            if (tool == "-" || tool == string.Empty)
            {
                continue;
            }

            var projects = Projects.FetchProjectsWithExperienceDevelopmentSupportingTool(tool);
            var years = Math.Round((this.FetchExperienceDays(projects) / 365.0), MidpointRounding.AwayFromZero);
            toolYears.Add((tool, years));
        }

        return toolYears.OrderByDescending(x => x.Year).ToList();
    }

    public IReadOnlyList<(string Tool, double Year)> GetExperienceOtherToolYears()
    {
        var tools = Projects.FetchExperienceOtherTools();

        var toolYears = new List<(string Tool, double Year)>();

        foreach (var tool in tools)
        {
            if (tool == "-" || tool == string.Empty)
            {
                continue;
            }

            var projects = Projects.FetchProjectsWithExperienceOtherTool(tool);
            var years = Math.Round((this.FetchExperienceDays(projects) / 365.0), MidpointRounding.AwayFromZero);
            toolYears.Add((tool, years));
        }

        return toolYears.OrderByDescending(x => x.Year).ToList();
    }

    public double FetchExperienceDays(IReadOnlyList<ProjectEntity> projects)
    {
        var days = default(double);

        foreach (var project in projects)
        {
            if (project.EndDate.Value.ToString("yyyy/MM/dd") == WorkingDateValue.Working.Value.ToString("yyyy/MM/dd"))
            {
                days += (DateTime.Today - project.StartDate.Value).TotalDays + 1;
            }
            else
            {
                days += (project.EndDate.Value - project.StartDate.Value).TotalDays + 1;
            }
        }

        return days;
    }

    public void ListView_SelectionChanged()
    {
        throw new NotImplementedException();
    }

    public void Reload()
    {
        throw new NotImplementedException();
    }
}
