using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace Medlebox.Models
{
    public class User
    {
        public User()
        {
            this.Gid = Guid.NewGuid();
        }
        public User(string email, string password)
            : this()
        {
            this.Email = email;
            SHA1 sha = new SHA1CryptoServiceProvider();
            this.PwdHash = sha.ComputeHash(Encoding.ASCII.GetBytes(password + email.ToLower()));
        }
        [Key]
        public Guid Gid { get; set; }
        [MaxLength(200)]
        [Required(ErrorMessage="Необходимо ввести почту")]
        public string Email { get; set; }
        [MaxLength(200)]
        public string Nickname { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string PasswordChange { get; set; }
        [NotMapped]
        public string PasswordConfirm { get; set; }
        [MaxLength(500)]
        public byte[] PwdHash { get; set; }
        [Range(0, 2, ErrorMessage = "К сожалению, данный источник не поддерживается")]
        public MusicSource MusicSource { get; set; }
        public string Name
        {
            get
            {
                return Nickname ?? Email;
            }
        }
        public bool TryLogin()
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] PwdHash = sha.ComputeHash(Encoding.ASCII.GetBytes(this.Password + this.Email.ToLower()));
            return PwdHash.SequenceEqual(this.PwdHash);
        }
        public void SetPwdHash()
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            this.PwdHash = sha.ComputeHash(Encoding.ASCII.GetBytes(this.PasswordChange + this.Email.ToLower()));
        }

        public virtual List<Playlist> PlayLists { get; set; }
        public virtual SongInPlaylist NowPlaying { get; set; }
    }

    public enum MusicSource
    {
        NotSet,
        [Display(Name = "vk.com")]
        vk,
        [Display(Name = "ololo.fm")]
        ololo,
        [Display(Name = "Yandex.Music")]
        yandex
    }
}