﻿namespace GameTools.NPCAccess;

/// <summary>
/// Describes the object that the NpcAccess service expects to receive 
/// when storing Npcs generated by teh Gamemaster Helper system.
/// </summary>
public class NpcAccessModel
{

    public NpcAccessModel()
    {
        Species = string.Empty;
        Vocation = string.Empty;
        CharacterDetails = string.Empty;
    }

    public int? NpcId { get; set; }

    public int UserId { get; set; }

    public string Species { get; set; }

    public string Vocation { get; set; }

    public string CharacterDetails { get; set; }

    public string? CharacterName { get; set; }

    public bool IsPublic { get; set; }
}