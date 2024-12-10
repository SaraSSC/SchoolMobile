using Prism.Commands;
using Prism.Mvvm;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace School.ViewModels
{
    public class MainViewModel 
    {
       
        private static MainViewModel instance;

        public TokenResponse Token { get; set; }

        public string Email { get; set; }

        public MainViewModel()
        {
            instance = this;
        }

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }

        public bool IsTokenValid()
        {
            if (Token != null)
            {
                if (!string.IsNullOrEmpty(Token.Token) && !string.IsNullOrEmpty(Token.Expiration))
                {
                    DateTime _expiration = new DateTime();

                    if (DateTime.TryParse(Token.Expiration, out _expiration))
                    {
                        if (_expiration >= DateTime.Now)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool IsEmailSaved()
        {
            if (!string.IsNullOrEmpty(Email))
            {
                return true;
            }

            return false;
        }
    }
}
    
