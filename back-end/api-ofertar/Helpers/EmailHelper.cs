using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
﻿using System.Net.Mail;
﻿using System.Net;

namespace api_ofertar.Helpers
{
    public class EmailHelper
    {
        private readonly IConfiguration _config;

        public EmailHelper(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmailRecovery(string toEmail, string token)
        {
            var smtpHost = _config["Email:SmtpHost"];
            var smtpPort = int.Parse(_config["Email:SmtpPort"]);
            var user = _config["Email:User"];
            var password = _config["Email:Password"];

            using var cliente = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(user, password),
                EnableSsl = true
            };

            var mensagem = new MailMessage(user, toEmail)
            {
                Subject = "Recuperação de Senha",
                Body = $"Clique no link para redefinir sua senha: {_config["Servidor:Url"]}atualizar-senha?token={token}",
                IsBodyHtml = true
            };

            cliente.Send(mensagem);
        }
    }
}