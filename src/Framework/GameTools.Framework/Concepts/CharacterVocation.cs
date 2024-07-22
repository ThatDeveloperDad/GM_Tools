using System;
using System.Collections.Generic;

namespace GameTools.Framework;

public sealed class CharacterVocation
{
    private readonly List<string> _skills;

    public CharacterVocation()
    {
        _skills = new List<string>();
        Name = string.Empty;
        Description = string.Empty;
        Title = string.Empty;
        LengthOfCareer = (decimal)0.0;
    }

    private string _name;
    public string Name 
    {
        get
        {
            return $"{_name}{RetiredValue}";
        }
        set
        {
            _name = value;
        } 
    }

    public string Description { get; set; }

    public string[] Skills => _skills.ToArray();

    public string Title { get; set; }

    public decimal LengthOfCareer { get; set; }

    public bool IsRetired { get; set; }

    private string RetiredValue => IsRetired ? " (Retired)" : string.Empty;

    public void AddSkill(string skill)
    {
        _skills.Add(skill);
    }

    public void AddSkills(string[] skills)
    {
        _skills.AddRange(skills);
    }
}
