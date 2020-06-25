
using Core.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Filters.Login
{
    public class UserLoginFilter
    {
        [Required]
        public string Email { get; set; }

        string _password;
        [Required]
        public string Password {
            get
            {
                return this._password;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value.Trim())) {
                    this._password = EncryptUtil.ApplyHash(value);
                }
            }
        }

    }

}