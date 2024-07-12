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

    public string Name { get; set; }

    public string Description { get; set; }

    public string[] Skills => _skills.ToArray();

    public string Title { get; set; }

    public decimal LengthOfCareer { get; set; }

    public void AddSkill(string skill)
    {
        _skills.Add(skill);
    }
}
