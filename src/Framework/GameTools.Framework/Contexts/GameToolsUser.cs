using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.Framework.Contexts;

namespace GameTools.Framework.Contexts
{
    public sealed class GameToolsUser : UserContextItem
    {
        public const string ItemTypeId = "GameToolsUser";
        private readonly List<string> _roles;


        public override string ContextKey { get => ItemTypeId; }

        public GameToolsUser(string userId)
        {
            _roles = new List<string>();
            UserId = userId;
        }

        public new string UserId { get; init; }

        public string? ScreenName { get; set; }

        public string[] Roles => _roles.ToArray();

        public void AddUserRole(string roleName)
        {
            _roles.Add(roleName);
        }

    }
}
