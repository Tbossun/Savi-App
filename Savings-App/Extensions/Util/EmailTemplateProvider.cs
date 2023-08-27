namespace Savings_App.Extensions.Util
{
    public static class EmailTemplateProvider
    {
        public static string GenerateConfirmationEmail(string userName, string confirmationLink)
        {
            string htmlBody = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f2f2f2;
                    color: #333;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #fff;
                    border-radius: 5px;
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    background-color: #007bff;
                    color: #fff;
                    padding: 10px;
                    text-align: center;
                    border-top-left-radius: 5px;
                    border-top-right-radius: 5px;
                }}
                .content {{
                    padding: 20px;
                }}
                .button {{
                    display: inline-block;
                    padding: 10px 20px;
                    background-color: #007bff;
                    color: #fff;
                    text-decoration: none;
                    border-radius: 5px;
                }}
            </style>
        </head>
        <body>
             <div class='container'>
                <div class='header'>
                    <h1>Email Confirmation</h1>
                </div>
                <div class='content'>
                    <p>Hello {userName},</p>
                    <p>Please confirm your email by clicking on the link below:</p>
                    <a class='button' href='{confirmationLink}'>Confirm Email</a>
                    <p>If you didn't request this, please ignore this email.</p>
                    <p>Thanks,</p>
                    <p>Savi Team</p>
                </div>
            </div>
        </body>
        </html>";

            return htmlBody;
        }


        public static string ResetPassword(string userName, string forgotPasswordLink)
        {
            string htmlBody = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f2f2f2;
                    color: #333;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #fff;
                    border-radius: 5px;
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    background-color: #007bff;
                    color: #fff;
                    padding: 10px;
                    text-align: center;
                    border-top-left-radius: 5px;
                    border-top-right-radius: 5px;
                }}
                .content {{
                    padding: 20px;
                }}
                .button {{
                    display: inline-block;
                    padding: 10px 20px;
                    background-color: #007bff;
                    color: #fff;
                    text-decoration: none;
                    border-radius: 5px;
                }}
            </style>
        </head>
        <body>
             <div class='container'>
                <div class='header'>
                    <h1>Email Confirmation</h1>
                </div>
                <div class='content'>
                    <p>Hello {userName},</p>
                    <p>Please Reset your Password by clicking on the link below:</p>
                    <a class='button' href='{forgotPasswordLink}'>Reset Password</a>
                    <p>If you didn't request this, please ignore this email.</p>
                    <p>Thanks,</p>
                    <p>Savi Team</p>
                </div>
            </div>
        </body>
        </html>";

            return htmlBody;
        }

        public static string OTP(string userName, string token)
        {
            string htmlBody = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f2f2f2;
                    color: #333;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #fff;
                    border-radius: 5px;
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    background-color: #007bff;
                    color: #fff;
                    padding: 10px;
                    text-align: center;
                    border-top-left-radius: 5px;
                    border-top-right-radius: 5px;
                }}
                .content {{
                    padding: 20px;
                }}
                .button {{
                    display: inline-block;
                    padding: 10px 20px;
                    background-color: #007bff;
                    color: #fff;
                    text-decoration: none;
                    border-radius: 5px;
                }}
            </style>
        </head>
        <body>
             <div class='container'>
                <div class='header'>
                    <h1>Email Confirmation</h1>
                </div>
                <div class='content'>
                    <p>Hello {userName},</p>
                    <p>Your Sign in Token is given below. Please ignore if you did not make this request</p>
                    <h1>{token} Login Token</h1>
                    <p>If you didn't request this, please ignore this email.</p>
                    <p>Thanks,</p>
                    <p>Savi Team</p>
                </div>
            </div>
        </body>
        </html>";

            return htmlBody;
        }

    }
}
