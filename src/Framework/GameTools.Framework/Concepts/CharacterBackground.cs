using System;
using System.Collections.Generic;

namespace GameTools.Framework.Concepts;

public sealed class CharacterBackground
{
    private readonly Dictionary<string, string> _traits;
    private readonly List<string> _skills;

    public CharacterBackground()
    {
        Name = string.Empty;
        Description = new GeneratedProperty();
        _traits = new Dictionary<string, string>();
        _skills = new List<string>();
    }

    public string Name { get; set; }

    public GeneratedProperty Description { get; set; }

    public Dictionary<string, string> Traits => _traits;

    public string[] Skills => _skills.ToArray();

    public void AddSkill(string skill)
    {
        _skills.Add(skill);
    }

    public void AddSkills(string[] skills)
    {
        _skills.AddRange(skills);
    }
}
