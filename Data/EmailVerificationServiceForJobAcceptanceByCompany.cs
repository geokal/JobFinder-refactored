using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class InternshipEmailService
{
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _supportEmail;
    private readonly string _noReplyEmail;

    public InternshipEmailService(string smtpUsername, string smtpPassword, string supportEmail, string noReplyEmail = null)
    {
        _smtpUsername = smtpUsername;
        _smtpPassword = smtpPassword;
        _supportEmail = supportEmail;
        _noReplyEmail = noReplyEmail ?? smtpUsername; // Fallback to SMTP username if no-reply not provided
    }

    public async Task SendVerificationEmail(
        string recipientEmail,
        string recipientName,
        string positionTitle,
        string companyName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, recipientName);

                string subject = $"Ένημέρωση σχετικά με την Αίτησή σας: {positionTitle} - {companyName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {recipientName},</p>

                    <p>Είμαστε στην ευχάριστη θέση να σας ενημερώσουμε ότι η αίτησή σας για τη θέση: 
                    <strong style='color: #1a73e8;'>{positionTitle}</strong> από: <strong>{companyName}</strong>, 
                    έχει γίνει <strong style='color: #34a853;'>Αποδεκτή</strong>!</p>

                    <p>Τα επόμενα βήματα θα σας κοινοποιηθούν σύντομα από τον αντίστοιχο υπεύθυνο.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>{companyName}</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send acceptance email: {ex.Message}");
            throw;
        }
    }

    public async Task SendRejectionEmail(
        string recipientEmail,
        string recipientName,
        string positionTitle,
        string companyName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, recipientName);

                string subject = $"Ένημέρωση σχετικά με την Αίτησή σας: {positionTitle} - {companyName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {recipientName},</p>

                    <p>Σας ευχαριστούμε για το ενδιαφέρον σας για: 
                    <strong style='color: #1a73e8;'>{positionTitle}</strong> - <strong>{companyName}</strong>.</p>

                    <p>Εκτιμούμε τον χρόνο και την προσπάθεια που καταβάλατε για την αίτησή σας, αλλά μετά από προσεκτική εξέταση, 
                    με λύπη σας ενημερώνουμε ότι αποφασίσαμε να προχωρήσουμε με άλλους υποψήφιους αυτή τη στιγμή.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>{companyName}</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send rejection email: {ex.Message}");
            throw;
        }
    }


    // New method: Send confirmation to student when he applies for a Company Job
    public async Task SendJobApplicationConfirmationToStudent(
        string studentEmail,
        string studentName,
        string studentSurname,
        string positionTitle,
        string jobRNG_Hashed,
        string companyName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(studentEmail, $"{studentName} {studentSurname}");

                string subject = $"Επιτυχής Αίτηση για Θέση: {positionTitle} - {companyName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {studentName} {studentSurname},</p>

                    <p>Η αίτησή σας για τη θέση με Κωδικό:
                        <strong style='color: red;'>{jobRNG_Hashed}</strong> και Τίτλο:
                        <strong style='color: #1a73e8;'>{positionTitle}</strong> στην εταιρεία: <strong>{companyName}</strong>, 
                        έχει υποβληθεί <strong style='color: #34a853;'>Επιτυχώς</strong>!</p>

                    <p>Η εταιρεία θα επανεξετάσει την αίτησή σας και θα ενημερωθείτε για την εξέλιξή της.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send student confirmation email: {ex.Message}");
            throw;
        }
    }

    // New method: Send notification to the Company when a Student applies for a Job
    public async Task SendJobApplicationNotificationToCompany(
    string companyEmail,
    string companyName,
    string studentName,
    string studentSurname,
    string studentEmail,
    string studentTelephone,
    string studentStudyYear,
    byte[] studentAttachment,
    string jobRNG_Hashed,
    string positionTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, companyName);

                string subject = $"Νέα Αίτηση για τη Θέση: {positionTitle}";

                string body = $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <p>Αγαπητοί/ές στην {companyName},</p>

                <p>Έχετε λάβει μια νέα αίτηση για τη θέση με Κωδικό: 
                    <strong style='color: red;'>{jobRNG_Hashed}</strong> και Τίτλο: 
                    <strong style='color: #1a73e8;'>{positionTitle}</strong>, από τον/την φοιτητή/τρια: 
                    <strong>{studentName} {studentSurname}</strong>.</p>

                <p>Στοιχεία φοιτητή:</p>
                <ul style='margin-top: 8px; padding-left: 20px;'>
                    <li>Email: {studentEmail}</li>
                    <li>Τηλέφωνο: {studentTelephone}</li>
                    <li>Έτος φοίτησης: {studentStudyYear}</li>
                </ul>

                <p>Μπορείτε να δείτε όλες τις αιτήσεις για αυτή τη θέση από τον πίνακα διαχείρισης σας.</p>

                <p>Με εκτίμηση,<br>
                <strong>AcadeMyHub</strong></p>

                <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                    <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                    <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                </div>
            </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    // Only add attachment if it exists and has content
                    if (studentAttachment != null && studentAttachment.Length > 0)
                    {
                        var attachment = new Attachment(
                            new MemoryStream(studentAttachment),
                            $"{studentName}_{studentSurname}_CV.pdf",
                            "application/pdf");

                        message.Attachments.Add(attachment);
                    }

                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send company notification email: {ex.Message}");
            throw;
        }
    }

    // New method: Send notification to the Student when he applies for a Company or Professor Thesis
    public async Task SendThesisApplicationConfirmationToStudent(
        string studentEmail,
        string studentName,
        string studentSurname,
        string thesisTitle,
        string thesisRNG_Hashed,
        string professorOrCompanyName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(studentEmail, $"{studentName} {studentSurname}");

                string subject = $"Επιτυχής Αίτηση για Πτυχιακή: {thesisTitle}";

                string body = $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <p>Αγαπητή/έ {studentName} {studentSurname},</p>

                <p>Η αίτησή σας για την πτυχιακή εργασία με Κωδικό Θέσης: 
                    <strong style='color: red;'>{thesisRNG_Hashed}</strong> και Τίτλο: 
                    <strong style='color: #1a73e8;'>{thesisTitle}</strong> από: <strong>{professorOrCompanyName}</strong>, 
                    έχει υποβληθεί <strong style='color: #34a853;'>Επιτυχώς</strong>!</p>

                <p>Ο/Η {professorOrCompanyName} θα επανεξετάσει την αίτησή σας και θα ενημερωθείτε για την εξέλιξή της.</p>

                <p>Με εκτίμηση,<br>
                <strong>AcadeMyHub</strong></p>

                <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                    <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                    <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                </div>
            </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis confirmation to student: {ex.Message}");
            throw;
        }
    }


    // New method: Send notification to the Professor when a Student applies for a Professor Thesis
    public async Task SendThesisApplicationNotificationToProfessor(
    string professorEmail,
    string professorName,
    string studentName,
    string studentSurname,
    string studentEmail,
    string studentTelephone,
    string studentStudyYear,
    string thesisRNG_Hashed,
    byte[] studentAttachment,  // Added attachment parameter
    string thesisTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(professorEmail, professorName);

                string subject = $"Νέα Αίτηση για Πτυχιακή: {thesisTitle}";

                string body = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <p>Αγαπητέ/ή Καθηγητή/ρια {professorName},</p>

            <p>Έχετε λάβει μια νέα αίτηση για την πτυχιακή εργασία με Κωδικό Θέσης: 
                <strong style='color: red;'>{thesisRNG_Hashed}</strong> και Τίτλο: 
                <strong style='color: #1a73e8;'>{thesisTitle}</strong> από τον/την φοιτητή/τρια: 
                <strong>{studentName} {studentSurname}</strong>.</p>

            <p>Στοιχεία φοιτητή:</p>
            <ul style='margin-top: 8px; padding-left: 20px;'>
                <li>Email: {studentEmail}</li>
                <li>Τηλέφωνο: {studentTelephone}</li>
                <li>Έτος φοίτησης: {studentStudyYear}</li>
            </ul>

            <p>Μπορείτε να δείτε όλες τις αιτήσεις από τον πίνακα διαχείρισης σας.</p>

            <p>Με εκτίμηση,<br>
            <strong>AcadeMyHub</strong></p>

            <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
            </div>
        </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    // Add attachment if it exists
                    if (studentAttachment != null && studentAttachment.Length > 0)
                    {
                        var attachment = new Attachment(
                            new MemoryStream(studentAttachment),
                            $"{studentName}_{studentSurname}_Thesis_Application.pdf",
                            "application/pdf");

                        message.Attachments.Add(attachment);
                    }

                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis notification to professor: {ex.Message}");
            throw;
        }
    }

    // New method: Send notification to the Company when a Student applies for a Company Thesis
    public async Task SendThesisApplicationNotificationToCompany(
    string companyEmail,
    string companyName,
    string studentName,
    string studentSurname,
    string studentEmail,
    string studentTelephone,
    string studentStudyYear,
    string thesisRNG_Hashed,
    byte[] studentAttachment,  
    string thesisTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, companyName);

                string subject = $"Νέα Αίτηση για Πτυχιακή: {thesisTitle}";

                string body = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <p>Αγαπητοί/ές στην {companyName},</p>

            <p>Έχετε λάβει μια νέα αίτηση για την πτυχιακή εργασία με Κωδικό Θέσης 
                <strong style='color: red;'>{thesisRNG_Hashed}</strong> και Τίτλο:
                <strong style='color: #1a73e8;'>{thesisTitle}</strong> από τον/την φοιτητή/τρια: 
                <strong>{studentName} {studentSurname}</strong>.</p>

            <p>Στοιχεία φοιτητή:</p>
            <ul style='margin-top: 8px; padding-left: 20px;'>
                <li>Email: {studentEmail}</li>
                <li>Τηλέφωνο: {studentTelephone}</li>
                <li>Έτος φοίτησης: {studentStudyYear}</li>
            </ul>

            <p>Μπορείτε να δείτε όλες τις αιτήσεις από τον πίνακα διαχείρισης σας.</p>

            <p>Με εκτίμηση,<br>
            <strong>AcadeMyHub</strong></p>

            <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
            </div>
        </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    // Add attachment if it exists
                    if (studentAttachment != null && studentAttachment.Length > 0)
                    {
                        var attachment = new Attachment(
                            new MemoryStream(studentAttachment),
                            $"{studentName}_{studentSurname}_Thesis_Application.pdf",
                            "application/pdf");

                        message.Attachments.Add(attachment);
                    }

                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis notification to company: {ex.Message}");
            throw;
        }
    }


    // New method: Send notification to the Student when he applies for a Company Internship
    public async Task SendCompanyInternshipApplicationConfirmationToStudent(
        string studentEmail,
        string studentName,
        string studentSurname,
        string internshipTitle,
        string internshipRNG_Hashed,
        string companyName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(studentEmail, $"{studentName} {studentSurname}");

                string subject = $"Επιτυχής Αίτηση για Πρακτική Άσκηση: {internshipTitle}";

                string body = $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <p>Αγαπητή/έ {studentName} {studentSurname},</p>

                <p>Η αίτησή σας για την πρακτική άσκηση με Κωδικό Θέσης: 
                    <strong style='color: red;'>{internshipRNG_Hashed}</strong> και Τίτλο:
                    <strong style='color: #1a73e8;'>{internshipTitle}</strong>, στην εταιρεία: <strong>{companyName}</strong>, 
                    έχει υποβληθεί <strong style='color: #34a853;'>Επιτυχώς</strong>!</p>

                <p>Η εταιρεία θα επανεξετάσει την αίτησή σας και θα ενημερωθείτε για την εξέλιξή της.</p>

                <p>Με εκτίμηση,<br>
                <strong>AcadeMyHub</strong></p>

                <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                    <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                    <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                </div>
            </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send internship confirmation to student: {ex.Message}");
            throw;
        }
    }

    // New method: Send notification to the Company when a Student applies for a Company Internship
    public async Task SendInternshipApplicationNotificationToCompany(
    string companyEmail,
    string companyName,
    string studentName,
    string studentSurname,
    string studentEmail,
    string studentTelephone,
    string studentStudyYear,
    byte[] studentAttachment,  // Added attachment parameter
    string internshipRNG_Hashed,
    string internshipTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, companyName);

                string subject = $"Νέα Αίτηση για Πρακτική Άσκηση: {internshipTitle}";

                string body = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <p>Αγαπητοί/ές στην {companyName},</p>

            <p>Έχετε λάβει μια νέα αίτηση για την πρακτική άσκηση με Κωδικό Θέσης: 
                <strong style='color: red;'>{internshipRNG_Hashed}</strong> και Τίτλο: 
                <strong style='color: #1a73e8;'>{internshipTitle}</strong>, από τον/την φοιτητή/τρια: 
                <strong>{studentName} {studentSurname}</strong>.</p>

            <p>Στοιχεία φοιτητή:</p>
            <ul style='margin-top: 8px; padding-left: 20px;'>
                <li>Email: {studentEmail}</li>
                <li>Τηλέφωνο: {studentTelephone}</li>
                <li>Έτος φοίτησης: {studentStudyYear}</li>
            </ul>

            <p>Μπορείτε να δείτε όλες τις αιτήσεις από τον πίνακα διαχείρισης σας.</p>

            <p>Με εκτίμηση,<br>
            <strong>AcadeMyHub</strong></p>

            <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
            </div>
        </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    // Add attachment if it exists
                    if (studentAttachment != null && studentAttachment.Length > 0)
                    {
                        var attachment = new Attachment(
                            new MemoryStream(studentAttachment),
                            $"{studentName}_{studentSurname}_Internship_Application.pdf",
                            "application/pdf");

                        message.Attachments.Add(attachment);
                    }

                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send internship notification to company: {ex.Message}");
            throw;
        }
    }


    // New method: Send notification to the Student when he applies for a Professor Internship
    public async Task SendProfessorInternshipApplicationConfirmationToStudent(
        string studentEmail,
        string studentName,
        string studentSurname,
        string internshipTitle,
        string internshipRNG_Hashed,
        string professorName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(studentEmail, $"{studentName} {studentSurname}");

                string subject = $"Επιτυχής Αίτηση για Πρακτική Άσκηση Καθηγητή: {internshipTitle}";

                string body = $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <p>Αγαπητή/έ {studentName} {studentSurname},</p>

                <p>Η αίτησή σας για την πρακτική άσκηση με Κωδικό Θέσης: 
                    <strong style='color: red;'>{internshipRNG_Hashed}</strong> και Τίτλο:
                    <strong style='color: #1a73e8;'>{internshipTitle}</strong>, με τον/την καθηγητή: <strong>{professorName}</strong>, 
                    έχει υποβληθεί <strong style='color: #34a853;'>Επιτυχώς</strong>!</p>

                <p>Ο/Η {professorName} θα επανεξετάσει την αίτησή σας και θα ενημερωθείτε για την εξέλιξή της.</p>

                <p>Με εκτίμηση,<br>
                <strong>AcadeMyHub</strong></p>

                <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                    <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                    <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                </div>
            </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send professor internship confirmation to student: {ex.Message}");
            throw;
        }
    }

    // New method: Send notification to the Professor when a Student applies for a Professor Internship
    public async Task SendProfessorInternshipApplicationNotificationToProfessor(
    string professorEmail,
    string professorName,
    string studentName,
    string studentSurname,
    string studentEmail,
    string studentTelephone,
    string studentStudyYear,
    byte[] studentAttachment,  // Added attachment parameter
    string internshipRNG_Hashed,
    string internshipTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(professorEmail, professorName);

                string subject = $"Νέα Αίτηση για Πρακτική Άσκηση: {internshipTitle}";

                string body = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <p>Αγαπητέ/ή Καθηγητή/ρια {professorName},</p>

            <p>Έχετε λάβει μια νέα αίτηση για την πρακτική άσκηση με Κωδικό Θέσης: 
                <strong style='color: red;'>{internshipRNG_Hashed}</strong> και Τίτλο: 
                <strong style='color: #1a73e8;'>{internshipTitle}</strong>, από τον/την φοιτητή/τρια: 
                <strong>{studentName} {studentSurname}</strong>.</p>

            <p>Στοιχεία φοιτητή:</p>
            <ul style='margin-top: 8px; padding-left: 20px;'>
                <li>Email: {studentEmail}</li>
                <li>Τηλέφωνο: {studentTelephone}</li>
                <li>Έτος φοίτησης: {studentStudyYear}</li>
            </ul>

            <p>Μπορείτε να δείτε όλες τις αιτήσεις από τον πίνακα διαχείρισης σας.</p>

            <p>Με εκτίμηση,<br>
            <strong>AcadeMyHub</strong></p>

            <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
            </div>
        </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    // Add attachment if it exists
                    if (studentAttachment != null && studentAttachment.Length > 0)
                    {
                        var attachment = new Attachment(
                            new MemoryStream(studentAttachment),
                            $"{studentName}_{studentSurname}_Internship_Application.pdf",
                            "application/pdf");

                        message.Attachments.Add(attachment);
                    }

                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send professor internship notification to professor: {ex.Message}");
            throw;
        }
    }


    public async Task SendConfirmationToStudentForInterestInCompanyEvent(
        string studentEmail,
        string studentName,
        string studentSurname,
        string eventTitle,
        string companyName,
        string eventRNG_Hashed,
        bool needsTransportForCompanyEvent,
        string transportLocation)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(studentEmail, $"{studentName} {studentSurname}");

                string subject = $"Επιτυχής Έκφραση Ενδιαφέροντος για Εκδήλωση: {eventTitle}";

                string transportInfo = needsTransportForCompanyEvent ?
                    $@"<p>Έχετε δηλώσει ανάγκη μετακίνησης από:</p>
                       <p style='margin-top: 4px; margin-bottom: 0;'>
                           <span style='vertical-align: middle;'>
                               <img src='data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAyNCAyNCI+PHBhdGggZmlsbD0iI0VBNTA0NSIgZD0iTTEyIDJDOC4xMyAyIDUgNS4xMyA1IDljMCA1LjI1IDcgMTMgNyAxM3M3LTcuNzUgNy0xM2MwLTMuODctMy4xMy03LTctN3ptMCA5LjVjLTEuMzggMC0yLjUtMS4xMi0yLjUtMi41czEuMTItMi41IDIuNS0yLjUgMi41IDEuMTIgMi41IDIuNS0xLjEyIDIuNS0yLjUgMi41eiIvPjwvc3ZnPg==' 
                                    alt='Map pin' 
                                    style='width:20px; height:20px; vertical-align:middle; margin-right:6px;'>
                               <a href='https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(transportLocation)}' 
                                  target='_blank' 
                                  style='color: #1a73e8; text-decoration: none; vertical-align: middle;'>
                                   <strong>{transportLocation}</strong>
                               </a>
                           </span>
                       </p>" :
                    "<p>Έχετε δηλώσει ότι <strong>δεν</strong> χρειάζεστε μετακίνηση.</p>";



                string body = $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <p>Αγαπητή/έ {studentName} {studentSurname},</p>

                <p>Η έκφραση ενδιαφέροντος σας για την εκδήλωση με Κωδικό: 
                    <strong style='color: red;'>{eventRNG_Hashed}</strong> και Τίτλο: 
                    <strong style='color: #1a73e8;'>{eventTitle}</strong>, της εταιρείας: <strong>{companyName}</strong>, 
                    έχει καταχωρηθεί <strong style='color: #34a853;'>Επιτυχώς</strong>!</p>

                {transportInfo}

                <p>Η εταιρεία θα επανεξετάσει το ενδιαφέρον σας και θα ενημερωθείτε για την εξέλιξή της.</p>

                <p>Με εκτίμηση,<br>
                <strong>AcadeMyHub</strong></p>

                <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                    <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                    <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                </div>
            </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send event interest confirmation to student: {ex.Message}");
            throw;
        }
    }

    public async Task SendNotificationToCompanyForStudentInterestForCompanyEvent(
    string companyEmail,
    string companyName,
    string studentName,
    string studentSurname,
    string studentEmail,
    string studentTelephone,
    string studentStudyYear,
    string eventTitle,
    string eventRNG_Hashed,
    bool needsTransportForCompanyEvent,
    string transportLocation)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, companyName);

                string subject = $"Νέα Έκφραση Ενδιαφέροντος για Εκδήλωση: {eventTitle}";

                string transportInfo = needsTransportForCompanyEvent ?
                    $@"<li style='margin-bottom: 6px;'>
                    Ανάγκη μετακίνησης: <strong>Ναι</strong>
                    <div style='margin-top: 4px;'>
                        <span style='vertical-align: middle;'>
                            <img src='data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAyNCAyNCI+PHBhdGggZmlsbD0iI0VBNTA0NSIgZD0iTTEyIDJDOC4xMyAyIDUgNS4xMyA1IDljMCA1LjI1IDcgMTMgNyAxM3M3LTcuNzUgNy0xM2MwLTMuODctMy4xNS03LTctN3ptMCA5LjVjLTEuMzggMC0yLjUtMS4xMi0yLjUtMi41czEuMTItMi41IDIuNS0yLjUgMi41IDEuMTIgMi41IDIuNS0xLjEyIDIuNS0yLjUgMi41eiIvPjwvc3ZnPg==' 
                                alt='Map pin' 
                                style='width:20px; height:20px; vertical-align:middle; margin-right:6px;'>
                            <a href='https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(transportLocation)}' 
                               target='_blank' 
                               style='color: #1a73e8; text-decoration: none; vertical-align: middle;'>
                                Από: <strong>{transportLocation}</strong>
                            </a>
                        </span>
                    </div>
                   </li>" :
                    "<li>Ανάγκη μετακίνησης: <strong>Όχι</strong></li>";

                string body = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <p>Αγαπητοί/ές {companyName},</p>

            <p>Έχετε λάβει μια νέα έκφραση ενδιαφέροντος για την εκδήλωση με Κωδικό: 
                <strong style='color: red;'>{eventRNG_Hashed}</strong> και Τίτλο: 
                <strong style='color: #1a73e8;'>{eventTitle}</strong>, από τον/την φοιτητή/τρια: 
                <strong>{studentName} {studentSurname}</strong>.</p>

            <p>Στοιχεία φοιτητή:</p>
            <ul style='margin-top: 8px; padding-left: 20px;'>
                <li>Email: {studentEmail}</li>
                <li>Τηλέφωνο: {studentTelephone}</li>
                <li>Έτος φοίτησης: {studentStudyYear}</li>
                {transportInfo}
            </ul>

            <p>Μπορείτε να δείτε όλες τις εκφράσεις ενδιαφέροντος από τον πίνακα διαχείρισης σας.</p>

            <p>Με εκτίμηση,<br>
            <strong>AcadeMyHub</strong></p>

            <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
            </div>
        </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send event interest notification to company: {ex.Message}");
            throw;
        }
    }

    public async Task SendConfirmationToStudentForInterestInProfessorEvent(
        string studentEmail,
        string studentName,
        string studentSurname,
        string eventTitle,
        string professorName,
        string professorSurname,
        string eventRNG_Hashed,
        bool needsTransport,
        string transportLocation)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(studentEmail, $"{studentName} {studentSurname}");

                string subject = $"Επιτυχής Έκφραση Ενδιαφέροντος για Εκδήλωση: {eventTitle}";

                string transportInfo = needsTransport ?
                    $@"<p>Έχετε δηλώσει ανάγκη μετακίνησης από:</p>
                   <p style='margin-top: 4px; margin-bottom: 0;'>
                       <span style='vertical-align: middle;'>
                           <img src='data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAyNCAyNCI+PHBhdGggZmlsbD0iI0VBNTA0NSIgZD0iTTEyIDJDOC4xMyAyIDUgNS4xMyA1IDljMCA1LjI1IDcgMTMgNyAxM3M3LTcuNzUgNy0xM2MwLTMuODctMy4xMy03LTctN3ptMCA5LjVjLTEuMzggMC0yLjUtMS4xMi0yLjUtMi41czEuMTItMi41IDIuNS0yLjUgMi41IDEuMTIgMi41IDIuNS0xLjEyIDIuNS0yLjUgMi41eiIvPjwvc3ZnPg==' 
                                alt='Map pin' 
                                style='width:20px; height:20px; vertical-align:middle; margin-right:6px;'>
                           <a href='https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(transportLocation)}' 
                              target='_blank' 
                              style='color: #1a73e8; text-decoration: none; vertical-align: middle;'>
                               <strong>{transportLocation}</strong>
                           </a>
                       </span>
                   </p>" :
                    "<p>Έχετε δηλώσει ότι <strong>δεν</strong> χρειάζεστε μετακίνηση.</p>";

                string body = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <p>Αγαπητή/έ {studentName} {studentSurname},</p>

            <p>Η έκφραση ενδιαφέροντος σας για την εκδήλωση με Κωδικό: 
                <strong style='color: red;'>{eventRNG_Hashed}</strong> και Τίτλο:
                <strong style='color: #1a73e8;'>{eventTitle}</strong> του καθηγητή: <strong>{professorName} {professorSurname}</strong>, 
                έχει καταχωρηθεί <strong style='color: #34a853;'>Επιτυχώς</strong>!</p>

            {transportInfo}

            <p>Ο/Η καθηγητής θα επανεξετάσει το ενδιαφέρον σας και θα ενημερωθείτε για την εξέλιξή της.</p>

            <p>Με εκτίμηση,<br>
            <strong>AcadeMyHub</strong></p>

            <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
            </div>
        </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send professor event interest confirmation to student: {ex.Message}");
            throw;
        }
    }

    public async Task SendNotificationToProfessorForStudentInterestForProfessorEvent(
     string professorEmail,
     string professorName,
     string professorSurname,
     string studentName,
     string studentSurname,
     string studentEmail,
     string studentTelephone,
     string studentStudyYear,
     string eventTitle,
     string eventRNG_Hashed,
     bool needsTransport,
     string transportLocation)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(professorEmail, $"{professorName} {professorSurname}");

                string subject = $"Νέα Έκφραση Ενδιαφέροντος για Εκδήλωση: {eventTitle}";

                string transportInfo = needsTransport ?
                    $@"<li style='margin-bottom: 6px;'>
                    Ανάγκη μετακίνησης: <strong>Ναι</strong>
                    <div style='margin-top: 4px;'>
                        <span style='vertical-align: middle;'>
                            <img src='data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAyNCAyNCI+PHBhdGggZmlsbD0iI0VBNTA0NSIgZD0iTTEyIDJDOC4xMyAyIDUgNS4xMyA1IDljMCA1LjI1IDcgMTMgNyAxM3M3LTcuNzUgNy0xM2MwLTMuODctMy4xNS03LTctN3ptMCA5LjVjLTEuMzggMC0yLjUtMS4xMi0yLjUtMi41czEuMTItMi41IDIuNS0yLjUgMi41IDEuMTIgMi41IDIuNS0xLjEyIDIuNS0yLjUgMi41eiIvPjwvc3ZnPg==' 
                                alt='Map pin' 
                                style='width:20px; height:20px; vertical-align:middle; margin-right:6px;'>
                            <a href='https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(transportLocation)}' 
                               target='_blank' 
                               style='color: #1a73e8; text-decoration: none; vertical-align: middle;'>
                                Από: <strong>{transportLocation}</strong>
                            </a>
                        </span>
                    </div>
                   </li>" :
                    "<li>Ανάγκη μετακίνησης: <strong>Όχι</strong></li>";

                string body = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <p>Αγαπητέ/ή Καθηγητή {professorName} {professorSurname},</p>

            <p>Έχετε λάβει μια νέα έκφραση ενδιαφέροντος για την εκδήλωση με Κωδικό: 
                <strong style='color: red;'>{eventRNG_Hashed}</strong> και Τίτλο:
                <strong style='color: #1a73e8;'>{eventTitle}</strong>, από τον/την φοιτητή/τρια: 
                <strong>{studentName} {studentSurname}</strong>.</p>

            <p>Στοιχεία φοιτητή:</p>
            <ul style='margin-top: 8px; padding-left: 20px;'>
                <li>Email: {studentEmail}</li>
                <li>Τηλέφωνο: {studentTelephone}</li>
                <li>Έτος φοίτησης: {studentStudyYear}</li>
                {transportInfo}
            </ul>

            <p>Μπορείτε να δείτε όλες τις εκφράσεις ενδιαφέροντος από τον πίνακα διαχείρισης σας.</p>

            <p>Με εκτίμηση,<br>
            <strong>AcadeMyHub</strong></p>

            <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
            </div>
        </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send professor event interest notification to professor: {ex.Message}");
            throw;
        }
    }


 // ==================================================================================================================================================================================//
 // ==================================================================================================================================================================================//
 // ==================================================================================================================================================================================//

    public async Task SendAcceptanceEmailAsCompanyToStudentAfterHeAppliedForJobPosition(
    string recipientEmail,
    string recipientName,
    string recipientSurname,
    string positionTitle,
    string companyName,
    string companyJob_HashedID,
    byte[] studentAttachment = null)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, $"{recipientName} {recipientSurname}");

                string subject = $"Αποδοχή Αίτησης για την Θέση Εργασίας: {positionTitle} - {companyName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {recipientName} {recipientSurname},</p>

                    <p>Είμαστε στην ευχάριστη θέση να σας ενημερώσουμε ότι η Αίτησή σας για τη Θέση Εργασίας με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{companyJob_HashedID}</strong> και Τίτλο:
                    <strong style='color: #1a73e8;'>{positionTitle}</strong>, στην Εταιρεία: <strong>{companyName}</strong>,
                    έχει γίνει <strong style='color: #34a853;'>Αποδεκτή</strong>!</p>

                    <p>Συνημμένο θα βρείτε το Βιογραφικό που υποβάλατε κατά την αίτησή σας.</p>

                    <p>Ο/Η υπεύθυνος της εταιρείας θα επικοινωνήσει μαζί σας σύντομα για τα επόμενα βήματα.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    // Add attachment if provided
                    if (studentAttachment != null && studentAttachment.Length > 0)
                    {
                        var attachment = new Attachment(
                            new MemoryStream(studentAttachment),
                            $"{recipientName}_{recipientSurname}_Application.pdf",
                            "application/pdf");

                        message.Attachments.Add(attachment);
                    }

                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send job acceptance email: {ex.Message}");
            throw;
        }
    }

    // Email to company
    public async Task SendAcceptanceConfirmationEmailToCompanyAfterStudentAppliedForJobPosition(
        string companyEmail,
        string companyName,
        string studentName,
        string studentSurname,
        string companyJob_HashedID,
        string positionTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, companyName);

                string subject = $"Επιβεβαίωση Αποδοχής Φοιτητή για την Θέση Εργασίας: {positionTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητοί/ές στην {companyName},</p>

                    <p>Επιβεβαιώνουμε ότι έχετε Αποδεχθεί την Aίτηση του/της φοιτητή/τριας: <strong>{studentName} {studentSurname}</strong> για τη Θέση Εργασίας με Τίτλο:
                    <strong style='color: #1a73e8;'>{positionTitle}</strong> και Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{companyJob_HashedID}</strong></p>

                    <p>Ο/Η φοιτητής/τρια έχει ειδοποιηθεί για την απόφασή σας και περιμένει τις οδηγίες σας 
                    για τα επόμενα βήματα της διαδικασίας.</p>

                    <p>Μπορείτε να δείτε όλες τις αποδεκτές αιτήσεις από τον πίνακα διαχείρισης σας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send company confirmation email: {ex.Message}");
            throw;
        }
    }


    public async Task SendRejectionConfirmationEmailToCompanyAfterStudentAppliedForJobPosition(
    string companyEmail,
    string companyName,
    string studentName,
    string studentSurname,
    string companyJob_HashedID,
    string positionTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, companyName);

                string subject = $"Απόρριψη Αίτησης Φοιτητή για Θέση Εργασίας: {positionTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητοί/ές στην {companyName},</p>

                    <p>Επιβεβαιώνουμε ότι Απορρίψατε την Αίτηση του/της φοιτητή/τριας:
                    <strong>{studentName} {studentSurname}</strong> για τη Θέση Εργασίας:
                    <strong style='color: #d93025;'>{positionTitle}</strong> με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{companyJob_HashedID}</strong></p>

                    <p>Ο/Η φοιτητής/τρια έχει ενημερωθεί για την απόφασή σας.</p>

                    <p>Μπορείτε να δείτε όλες τις Αιτήσεις στον πίνακα διαχείρισης σας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send company rejection email: {ex.Message}");
            throw;
        }
    }

    public async Task SendRejectionEmailAsCompanyToStudentAfterHeAppliedForJobPosition(
    string recipientEmail,
    string recipientName,
    string recipientSurname,
    string positionTitle,
    string companyName,
    string companyJob_HashedID,
    byte[] studentAttachment = null)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, $"{recipientName} {recipientSurname}");

                string subject = $"Απόρριψη Αίτησης για την Θέση Εργασίας: {positionTitle} - {companyName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {recipientName} {recipientSurname},</p>

                    <p>Σας ευχαριστούμε για το ενδιαφέρον σας και την Αίτησή σας για τη Θέση Εργασίας με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{companyJob_HashedID}</strong> και Τίτλο:
                    <strong style='color: #1a73e8;'>{positionTitle}</strong>, στην εταιρεία:
                    <strong>{companyName}</strong>.</p>

                    <p>Θα θέλαμε να σας ενημερώσουμε ότι, μετά από προσεκτική αξιολόγηση, αποφασίσαμε να Μην Προχωρήσουμε με την Aίτησή σας αυτή τη φορά.</p>

                    <p>Σας ενθαρρύνουμε να συνεχίσετε την προσπάθειά σας και να υποβάλετε αιτήσεις σε άλλες θέσεις που σας ενδιαφέρουν στο μέλλον.</p>

                    <p>Σας ευχόμαστε κάθε επιτυχία στην επαγγελματική σας πορεία.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    // Optionally attach student file
                    if (studentAttachment != null && studentAttachment.Length > 0)
                    {
                        var attachment = new Attachment(
                            new MemoryStream(studentAttachment),
                            $"{recipientName}_{recipientSurname}_Application.pdf",
                            "application/pdf");

                        message.Attachments.Add(attachment);
                    }

                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send job rejection email: {ex.Message}");
            throw;
        }
    }

    public async Task SendAcceptanceEmailAsCompanyToStudentAfterHeAppliedForInternshipPosition(
    string recipientEmail,
    string recipientName,
    string recipientSurname,
    string positionTitle,
    string companyName,
    string companyInternship_HashedID,
    byte[] studentAttachment = null)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, $"{recipientName} {recipientSurname}");

                string subject = $"Αποδοχή Αίτησης για την Θέση Πρακτικής Άσκησης: {positionTitle} - {companyName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {recipientName} {recipientSurname},</p>

                    <p>Είμαστε στην ευχάριστη θέση να σας ενημερώσουμε ότι η Αίτησή σας για την Πρακτική Άσκηση με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{companyInternship_HashedID}</strong> και Τίτλο:
                    <strong style='color: #1a73e8;'>{positionTitle}</strong>, στην εταιρεία: <strong>{companyName}</strong>,
                    έχει γίνει <strong style='color: #34a853;'>Αποδεκτή</strong>!</p>

                    <p>Συνημμένο θα βρείτε το Βιογραφικό που υποβάλατε κατά την αίτησή σας.</p>

                    <p>Ο/Η υπεύθυνος της εταιρείας θα επικοινωνήσει μαζί σας σύντομα για τα επόμενα βήματα.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    if (studentAttachment != null && studentAttachment.Length > 0)
                    {
                        var attachment = new Attachment(
                            new MemoryStream(studentAttachment),
                            $"{recipientName}_{recipientSurname}_Application.pdf",
                            "application/pdf");

                        message.Attachments.Add(attachment);
                    }

                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send internship acceptance email: {ex.Message}");
            throw;
        }
    }

    public async Task SendAcceptanceConfirmationEmailToCompanyAfterStudentAppliedForInternshipPosition(
    string companyEmail,
    string companyName,
    string studentName,
    string studentSurname,
    string companyInternship_HashedID,
    string internshipTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, companyName);

                string subject = $"Αποδοχή Αίτησης για Πρακτική Θέση: {internshipTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητοί εκπρόσωποι της <strong>{companyName}</strong>,</p>

                    <p>Ο φοιτητής <strong style='color: #1a73e8;'>{studentName} {studentSurname}</strong> έγινε Αποδεκτός/ή για την Πρακτική Άσκηση με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{companyInternship_HashedID}</strong> και Τίτλο:
                    <strong>{internshipTitle}</strong>.</p>

                    <p>Η Αποδοχή αυτή έχει καταγραφεί Επιτυχώς στην πλατφόρμα <strong>AcadeMyHub</strong>.</p>

                    <p>Σας ευχαριστούμε που χρησιμοποιείτε την πλατφόρμα μας για τη συνεργασία σας με την ακαδημαϊκή κοινότητα.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send internship acceptance confirmation to company: {ex.Message}");
            throw;
        }
    }


    public async Task SendRejectionEmailAsCompanyToStudentAfterHeAppliedForInternshipPosition(
    string recipientEmail,
    string recipientName,
    string recipientSurname,
    string positionTitle,
    string companyName,
    string companyInternship_HashedID,
    byte[] studentAttachment = null)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, $"{recipientName} {recipientSurname}");

                string subject = $"Απόρριψη Αίτησης για Πρακτική Άσκηση: {positionTitle} - {companyName}";

                string body = $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <p>Αγαπητή/έ {recipientName} {recipientSurname},</p>

                <p>Σας ευχαριστούμε για το ενδιαφέρον σας και την Αίτησή σας για την Πρακτική Άσκηση με Κωδικό:
                <strong style='color: #ff0000; font-weight: bold;'>{companyInternship_HashedID}</strong> και Τίτλο:
                <strong style='color: #1a73e8;'>{positionTitle}</strong>, στην Εταιρεία: <strong>{companyName}</strong>.</p>

                <p>Θα θέλαμε να σας ενημερώσουμε ότι, μετά από προσεκτική αξιολόγηση, αποφασίσαμε να Μην Προχωρήσουμε με την Αίτησή σας αυτή τη φορά.</p>

                <p>Συνεχίστε την προσπάθεια σας και εξετάστε άλλες διαθέσιμες θέσεις στο AcadeMyHub.</p>

                <p>Σας ευχόμαστε κάθε επιτυχία στην ακαδημαϊκή και επαγγελματική σας πορεία.</p>

                <p>Με εκτίμηση,<br>
                <strong>AcadeMyHub</strong></p>

                <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                    <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                    <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                </div>
            </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    if (studentAttachment != null && studentAttachment.Length > 0)
                    {
                        var attachment = new Attachment(
                            new MemoryStream(studentAttachment),
                            $"{recipientName}_{recipientSurname}_Application.pdf",
                            "application/pdf");

                        message.Attachments.Add(attachment);
                    }

                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send internship rejection email: {ex.Message}");
            throw;
        }
    }


    public async Task SendRejectionConfirmationEmailToCompanyAfterStudentAppliedForInternshipPosition(
    string companyEmail,
    string companyName,
    string studentName,
    string studentSurname,
    string companyInternship_HashedID,
    string positionTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, companyName);

                string subject = $"Απόρριψη Αίτησης Φοιτητή για Πρακτική Άσκηση: {positionTitle}";

                string body = $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <p>Αγαπητοί/ές στην {companyName},</p>

                <p>Επιβεβαιώνουμε ότι απορρίψατε την αίτηση του/της φοιτητή/τριας: <strong>{studentName} {studentSurname}</strong> για την πρακτική άσκηση με Κωδικό:
                <strong style='color: #ff0000; font-weight: bold;'>{companyInternship_HashedID}</strong> και Τίτλο:
                <strong style='color: #d93025;'>{positionTitle}</strong>.</p>

                <p>Ο/Η φοιτητής/τρια έχει ενημερωθεί για την απόφασή σας.</p>

                <p>Μπορείτε να δείτε όλες τις Αιτήσεις στον Πίνακα Διαχείρισης σας, στις Θέσεις Πρακτικής Άσκησης.</p>

                <p>Με εκτίμηση,<br>
                <strong>AcadeMyHub</strong></p>

                <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                    <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                    <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                </div>
            </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send internship company rejection email: {ex.Message}");
            throw;
        }
    }

    public async Task SendAcceptanceEmailAsCompanyToStudentAfterHeAppliedForThesisPosition(
    string recipientEmail,
    string recipientName,
    string recipientSurname,
    string thesisTitle,
    string companyName,
    string companyThesis_HashedID,
    byte[] studentAttachment = null)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, $"{recipientName} {recipientSurname}");

                string subject = $"Αποδοχή Αίτησης για την Διπλωματική Εργασία: {thesisTitle} - {companyName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {recipientName} {recipientSurname},</p>

                    <p>Είμαστε στην ευχάριστη θέση να σας ενημερώσουμε ότι η αίτησή σας για τη Διπλωματική Εργασία με Κωδικό: <strong style='color: #ff0000; font-weight: bold;'>{companyThesis_HashedID}</strong> και Τίτλο: 
                    <strong style='color: #1a73e8;'>{thesisTitle}</strong> στην εταιρεία: <strong>{companyName}</strong>, 
                    έχει γίνει <strong style='color: #34a853;'>Αποδεκτή</strong>!</p>

                    <p>Συνημμένο θα βρείτε το Βιογραφικό που υποβάλατε κατά την αίτησή σας.</p>

                    <p>Ο/Η υπεύθυνος της εταιρείας θα επικοινωνήσει μαζί σας σύντομα για τα επόμενα βήματα.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    if (studentAttachment != null && studentAttachment.Length > 0)
                    {
                        var attachment = new Attachment(
                            new MemoryStream(studentAttachment),
                            $"{recipientName}_{recipientSurname}_Thesis_Application.pdf",
                            "application/pdf");

                        message.Attachments.Add(attachment);
                    }

                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis acceptance email: {ex.Message}");
            throw;
        }
    }

    public async Task SendAcceptanceConfirmationEmailToCompanyAfterStudentAppliedForThesisPosition(
        string companyEmail,
        string companyName,
        string studentName,
        string studentSurname,
        string companyThesis_HashedID,
        string thesisTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, companyName);

                string subject = $"Αποδοχή Αίτησης για Διπλωματική Εργασία: {thesisTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητοί εκπρόσωποι της <strong>{companyName}</strong>,</p>

                    <p>Ο φοιτητής <strong style='color: #1a73e8;'>{studentName} {studentSurname}</strong> έγινε Aποδεκτός/ή για τη διπλωματική εργασία: 
                    <strong>{thesisTitle}</strong>.</p> 
                    με Κωδικό: <strong style='color: #ff0000; font-weight: bold;'>{companyThesis_HashedID}</strong>.

                    <p>Η αποδοχή αυτή έχει καταγραφεί Επιτυχώς στην πλατφόρμα <strong>AcadeMyHub</strong>.</p>

                    <p>Σας ευχαριστούμε που χρησιμοποιείτε την πλατφόρμα μας για τη συνεργασία σας με την Ακαδημαϊκή κοινότητα.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis acceptance confirmation to company: {ex.Message}");
            throw;
        }
    }

    public async Task SendRejectionEmailAsCompanyToStudentAfterHeAppliedForThesisPosition(
    string recipientEmail,
    string recipientName,
    string recipientSurname,
    string thesisTitle,
    string companyThesis_HashedID,
    string companyName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, $"{recipientName} {recipientSurname}");

                string subject = $"Απόρριψη Αίτησης για Διπλωματική Εργασία: {thesisTitle} - {companyName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {recipientName} {recipientSurname},</p>

                    <p>Λυπούμαστε να σας ενημερώσουμε ότι η αίτησή σας για τη Διπλωματική Εργασία με Κωδικό: 
                    <strong style='color: #ff0000; font-weight: bold;'>{companyThesis_HashedID}</strong>
                    και Τίτλο: <strong style='color: #1a73e8;'>{thesisTitle}</strong>, στην εταιρεία: <strong>{companyName}</strong>, έχει <strong style='color: #ea4335;'>απορριφθεί</strong>.</p>

                    <p>Αυτό δεν σημαίνει ότι δεν έχετε τις απαραίτητες ικανότητες, αλλά ότι η συγκεκριμένη θέση 
                    δεν ταιριάζει με το προφίλ σας ή υπάρχουν άλλοι υποψήφιοι που ταιριάζουν καλύτερα.</p>

                    <p>Σας ενθαρρύνουμε να συνεχίσετε να ψάχνετε για άλλες ευκαιρίες στην πλατφόρμα μας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis rejection email: {ex.Message}");
            throw;
        }
    }

    public async Task SendRejectionConfirmationEmailToCompanyAfterStudentAppliedForThesisPosition(
        string companyEmail,
        string companyName,
        string studentName,
        string studentSurname,
        string companyThesis_HashedID,
        string thesisTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, companyName);

                string subject = $"Απόρριψη Αίτησης για Διπλωματική Εργασία: {thesisTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητοί εκπρόσωποι της <strong>{companyName}</strong>,</p>

                    <p>Ο φοιτητής <strong style='color: #1a73e8;'>{studentName} {studentSurname}</strong> έχει Απορριφθεί για τη Διπλωματική εργασία με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{companyThesis_HashedID}</strong>
                    και Τίτλο: <strong>{thesisTitle}</strong>.</p>

                    <p>Η απόρριψη αυτή έχει καταγραφεί επιτυχώς στην πλατφόρμα <strong>AcadeMyHub</strong>.</p>

                    <p>Σας ευχαριστούμε που χρησιμοποιείτε την πλατφόρμα μας για τη συνεργασία σας με την ακαδημαϊκή κοινότητα.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis rejection confirmation to company: {ex.Message}");
            throw;
        }
    }

    public async Task SendAcceptanceEmailAsProfessorToStudentAfterHeAppliedForThesisPosition(
    string recipientEmail,
    string recipientName,
    string recipientSurname,
    string thesisTitle,
    string professorThesis_HashedID,
    string professorName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, $"{recipientName} {recipientSurname}");

                string subject = $"Αποδοχή Αίτησης για Διπλωματική Εργασία: {thesisTitle} - Καθηγητής {professorName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {recipientName} {recipientSurname},</p>

                    <p>Είμαστε στην ευχάριστη θέση να σας ενημερώσουμε ότι η αίτησή σας για τη Διπλωματική Εργασία με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{professorThesis_HashedID}</strong> και Τίτλο:
                    <strong style='color: #1a73e8;'>{thesisTitle}</strong>, υπό τον Επιβλέποντα Καθηγητή: <strong>Καθηγητή {professorName}</strong>,
                    έχει γίνει <strong style='color: #34a853;'>Αποδεκτή</strong>!</p>

                    <p>Ο/Η Καθηγητής {professorName} θα επικοινωνήσει μαζί σας σύντομα για τα επόμενα βήματα.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis acceptance email: {ex.Message}");
            throw;
        }
    }

    public async Task SendAcceptanceConfirmationEmailToProfessorAfterStudentAppliedForThesisPosition(
        string professorEmail,
        string professorName,
        string studentName,
        string studentSurname,
        string professorThesis_HashedID,
        string thesisTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(professorEmail, $"Καθηγητής {professorName}");

                string subject = $"Αποδοχή Αίτησης για Διπλωματική Εργασία: {thesisTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητέ/ή Καθηγητά/τρια <strong>{professorName}</strong>,</p>

                    <p>Ο φοιτητής/ρια <strong style='color: #1a73e8;'>{studentName} {studentSurname}</strong> έγινε Αποδεκτός/ή για την Διπλωματική Εργασία με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{professorThesis_HashedID}</strong> και Τίτλο:
                    <strong>{thesisTitle}</strong>.</p>

                    <p>Η αποδοχή αυτή έχει καταγραφεί Επιτυχώς στην πλατφόρμα <strong>AcadeMyHub</strong>.</p>

                    <p>Παρακαλώ επικοινωνήστε με τον φοιτητή για τα επόμενα βήματα.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis acceptance confirmation to professor: {ex.Message}");
            throw;
        }
    }

    public async Task SendRejectionEmailAsProfessorToStudentAfterHeAppliedForThesisPosition(
    string recipientEmail,
    string recipientName,
    string recipientSurname,
    string thesisTitle,
    string professorThesis_HashedID,
    string professorName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, $"{recipientName} {recipientSurname}");

                string subject = $"Απόρριψη Αίτησης για Διπλωματική Εργασία: {thesisTitle} - Καθηγητής {professorName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {recipientName} {recipientSurname},</p>

                    <p>Λυπούμαστε να σας ενημερώσουμε ότι η αίτησή σας για τη Διπλωματική Εργασία με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{professorThesis_HashedID}</strong> και Τίτλο:
                    <strong style='color: #1a73e8;'>{thesisTitle}</strong>, υπό τον Επιβλέποντα Καθηγητή: <strong>{professorName}</strong>,
                    έχει <strong style='color: #ea4335;'>Απορριφθεί</strong>.</p>

                    <p>Αυτό δεν σημαίνει ότι δεν έχετε τις απαραίτητες ικανότητες, αλλά ότι η συγκεκριμένη θέση 
                    δεν ταιριάζει με το προφίλ σας ή ο καθηγητής έχει περιορισμένη διαθεσιμότητα.</p>

                    <p>Σας ενθαρρύνουμε να συνεχίσετε να ψάχνετε για άλλες ευκαιρίες στην πλατφόρμα μας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis rejection email: {ex.Message}");
            throw;
        }
    }

    public async Task SendRejectionConfirmationEmailToProfessorAfterStudentAppliedForThesisPosition(
        string professorEmail,
        string professorName,
        string studentName,
        string studentSurname,
        string professorThesis_HashedID,
        string thesisTitle)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(professorEmail, $"Καθηγητής {professorName}");

                string subject = $"Απόρριψη Αίτησης για Διπλωματική Εργασία: {thesisTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητέ/ή Καθηγητά/τρια <strong>{professorName}</strong>,</p>

                    <p>Ο φοιτητής/ρια <strong style='color: #1a73e8;'>{studentName} {studentSurname}</strong> έχει Απορριφθεί για τη Διπλωματική Εργασία με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{professorThesis_HashedID}</strong> και Τίτλο:
                    <strong>{thesisTitle}</strong>.</p>

                    <p>Η Απόρριψη αυτή έχει καταγραφεί επιτυχώς στην πλατφόρμα <strong>AcadeMyHub</strong>.</p>

                    <p>Εάν επιθυμείτε να αλλάξετε αυτή την απόφαση, μπορείτε να επικοινωνήσετε απευθείας με τον φοιτητή.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis rejection confirmation to professor: {ex.Message}");
            throw;
        }
    }

    public async Task SendAcceptanceEmailAsProfessorToStudentAfterHeAppliedForInternshipPosition(
    string recipientEmail,
    string recipientName,
    string recipientSurname,
    string internshipTitle,
    string professorInternship_HashedID,
    string professorName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
                throw new ArgumentException("Recipient email cannot be empty");

            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, $"{recipientName} {recipientSurname}");

                string subject = $"Αποδοχή Αίτησης για Πρακτική Άσκηση: {internshipTitle} - Καθηγητής {professorName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {recipientName} {recipientSurname},</p>

                    <p>Είμαστε στην ευχάριστη θέση να σας ενημερώσουμε ότι η Αίτησή σας για την Πρακτική Άσκηση με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{professorInternship_HashedID}</strong> και Τίτλο:
                    <strong style='color: #1a73e8;'>{internshipTitle}</strong>, υπό τον Επιβλέποντα Καθηγητή:
                    <strong>Καθηγητή {professorName}</strong>, έχει γίνει <strong style='color: #34a853;'>Αποδεκτή</strong>!</p>

                    <p>Ο/Η καθηγητής {professorName} θα επικοινωνήσει μαζί σας σύντομα για τα επόμενα βήματα.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send internship acceptance email: {ex.Message}");
            throw;
        }
    }

    public async Task SendAcceptanceConfirmationEmailToProfessorAfterStudentAppliedForInternshipPosition(
        string professorEmail,
        string professorName,
        string studentName,
        string studentSurname,
        string professorInternship_HashedID,
        string internshipTitle)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(professorEmail))
                throw new ArgumentException("Professor email cannot be empty");

            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(professorEmail, professorName);

                string subject = $"Αποδοχή Αίτησης για Πρακτική Άσκηση: {internshipTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητέ/ή Καθηγητά/τρια <strong>{professorName}</strong>,</p>

                    <p>Ο φοιτητής <strong style='color: #1a73e8;'>{studentName} {studentSurname}</strong> έγινε Αποδεκτός/ή για την Πρακτική Άσκηση με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{professorInternship_HashedID}</strong> και Τίτλο:
                    <strong>{internshipTitle}</strong>.</p>

                    <p>Η αποδοχή αυτή έχει καταγραφεί επιτυχώς στην πλατφόρμα <strong>AcadeMyHub</strong>.</p>

                    <p>Παρακαλώ επικοινωνήστε με τον φοιτητή για τα επόμενα βήματα.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send internship acceptance confirmation to professor: {ex.Message}");
            throw;
        }
    }

    public async Task SendRejectionEmailAsProfessorToStudentAfterHeAppliedForInternshipPosition(
    string recipientEmail,
    string recipientName,
    string recipientSurname,
    string internshipTitle,
    string professorInternship_HashedID,
    string professorName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
                throw new ArgumentException("Recipient email cannot be empty");

            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(recipientEmail, $"{recipientName} {recipientSurname}");

                string subject = $"Απόρριψη Αίτησης για Πρακτική Άσκηση: {internshipTitle} - Καθηγητής {professorName}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {recipientName} {recipientSurname},</p>

                    <p>Λυπούμαστε να σας ενημερώσουμε ότι η αίτησή σας για την Πρακτική Άσκηση με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{professorInternship_HashedID}</strong> και Τίτλο:
                    <strong style='color: #1a73e8;'>{internshipTitle}</strong>, υπό τον Επιβλέποντα Καθηγητή:
                    <strong>Καθηγητή {professorName}</strong>, έχει <strong style='color: #ea4335;'>Απορριφθεί</strong>.</p>

                    <p>Αυτό δεν σημαίνει ότι δεν έχετε τις απαραίτητες ικανότητες, αλλά ότι η συγκεκριμένη θέση 
                    δεν ταιριάζει με το προφίλ σας ή ο καθηγητής έχει περιορισμένη διαθεσιμότητα.</p>

                    <p>Σας ενθαρρύνουμε να συνεχίσετε να ψάχνετε για άλλες ευκαιρίες στην πλατφόρμα μας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send internship rejection email: {ex.Message}");
            throw;
        }
    }

    public async Task SendRejectionConfirmationEmailToProfessorAfterStudentAppliedForInternshipPosition(
        string professorEmail,
        string professorName,
        string studentName,
        string studentSurname,
        string professorInternship_HashedID,
        string internshipTitle)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(professorEmail))
                throw new ArgumentException("Professor email cannot be empty");

            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(professorEmail, professorName);

                string subject = $"Απόρριψη Αίτησης για Πρακτική Άσκηση: {internshipTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητέ/ή Καθηγητά/τρια <strong>{professorName}</strong>,</p>

                    <p>Ο φοιτητής <strong style='color: #1a73e8;'>{studentName} {studentSurname}</strong> έχει Απορριφθεί για την Πρακτική Άσκηση με Κωδικό:
                    <strong style='color: #ff0000; font-weight: bold;'>{professorInternship_HashedID}</strong> και Τίτλο:
                    <strong>{internshipTitle}</strong>.</p>


                    <p>Η Απόρριψη αυτή έχει καταγραφεί επιτυχώς στην πλατφόρμα <strong>AcadeMyHub</strong>.</p>

                    <p>Εάν επιθυμείτε να αλλάξετε αυτή την απόφαση, μπορείτε να επικοινωνήσετε απευθείας με τον φοιτητή.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send internship rejection confirmation to professor: {ex.Message}");
            throw;
        }
    }

    public async Task SendStudentThesisWithdrawalNotificationToCompanyOrProfessor(
        string recipientEmail,
        string recipientName,
        string studentName,
        string studentSurname,
        string thesisTitle,
        string thesisRNG_Hashed)
        {
            try
            {
                using (var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    Timeout = 30000
                })
                {
                    var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                    var toAddress = new MailAddress(recipientEmail, recipientName);

                    string subject = $"Απόσυρση Αίτησης για την Πτυχιακή: {thesisTitle}";

                    string body = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                        <p>Αγαπητοί/ές {recipientName},</p>

                        <p>Ο/Η φοιτητής/τρια <strong>{studentName} {studentSurname}</strong> έχει αποσύρει την αίτησή του/της για την Πτυχιακή Εργασία με:</p>
    
                        <ul style='margin-top: 8px; padding-left: 20px;'>
                            <li>Κωδικό Θέσης: <strong style='color: red;'>{thesisRNG_Hashed}</strong></li>
                            <li>Τίτλο: <strong style='color: #1a73e8;'>{thesisTitle}</strong></li>
                        </ul>

                        <p>Ο/Η Ενδιαφερόμενος/η φοιτητής/ρια θα συνεχίσει να εμφανίζεται στους Αιτούντες της Αντίστοιχης Πτυχιακής στον πίνακα διαχείρισής σας.</p>

                        <p>Με εκτίμηση,<br>
                        <strong>AcadeMyHub</strong></p>

                        <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                            <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                            <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                        </div>
                    </div>";

                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                        ReplyToList = { new MailAddress(_supportEmail) }
                    })
                    {
                        await smtp.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send thesis withdrawal notification to recipient: {ex.Message}");
                throw;
            }
    }

    public async Task SendStudentThesisWithdrawalConfirmationToStudent(
    string studentEmail,
    string studentName,
    string studentSurname,
    string thesisTitle,
    string thesisRNG_Hashed,
    string recipientName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(studentEmail, $"{studentName} {studentSurname}");

                string subject = $"Επιτυχής Απόσυρση από την Πτυχιακή: {thesisTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {studentName} {studentSurname},</p>

                    <p>Έχετε Επιτυχώς Αποσύρει την Αίτησή σας για την Πτυχιακή Εργασία:</p>
    
                    <ul style='margin-top: 8px; padding-left: 20px;'>
                        <li>Κωδικό Θέσης: <strong style='color: red;'>{thesisRNG_Hashed}</strong></li>
                        <li>Τίτλο: <strong style='color: #1a73e8;'>{thesisTitle}</strong></li>
                        <li>Από: <strong>{recipientName}</strong></li>
                    </ul>

                    <p>Η ενέργειά αυτή είναι οριστική, αλλά η Αίτησή σας θα συνεχίζει να εμφανίζεται στον πίνακα Διαχείρισης των Αιτήσεών σας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send thesis withdrawal confirmation to student: {ex.Message}");
            throw;
        }
    }

    public async Task SendJobWithdrawalNotificationToCompany_AsStudent(
        string companyEmail,
        string companyName,
        string studentName,
        string studentSurname,
        string positionTitle,
        string positionRNG_Hashed)
        {
            try
            {
                using (var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    Timeout = 30000
                })
                {
                    var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                    var toAddress = new MailAddress(companyEmail, companyName);

                    string subject = $"Απόσυρση Αίτησης για την Θέση Εργασίας: {positionTitle}";

                    string body = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                        <p>Αγαπητοί/ές στην {companyName},</p>

                        <p>Ο/Η φοιτητής/τρια <strong>{studentName} {studentSurname}</strong> έχει Αποσύρει την αίτησή του/της για την Θέση Εργασίας:</p>
    
                        <ul style='margin-top: 8px; padding-left: 20px;'>
                            <li>Κωδικό Θέσης: <strong style='color: red;'>{positionRNG_Hashed}</strong></li>
                            <li>Τίτλο: <strong style='color: #1a73e8;'>{positionTitle}</strong></li>
                        </ul>

                        <p>Αυτή η ενέργεια είναι οριστική και ο/η Αιτούντας θα συνεχίζει να εμφανίζεται στον Πίνακα Διαχείρισής των Θέσεων Εργασίας σας.</p>

                        <p>Με εκτίμηση,<br>
                        <strong>AcadeMyHub</strong></p>

                        <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                            <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                            <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                        </div>
                    </div>";

                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                        ReplyToList = { new MailAddress(_supportEmail) }
                    })
                    {
                        await smtp.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send job withdrawal notification to company: {ex.Message}");
                throw;
            }
        }

    public async Task SendJobWithdrawalConfirmationToStudent_AsCompany(
        string studentEmail,
        string studentName,
        string studentSurname,
        string positionTitle,
        string positionRNG_Hashed,
        string companyName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(studentEmail, $"{studentName} {studentSurname}");

                string subject = $"Επιτυχής Απόσυρση από την Θέση Εργασίας: {positionTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {studentName} {studentSurname},</p>

                    <p>Έχετε επιτυχώς Αποσύρει την Αίτησή σας για την Θέση Εργασίας:</p>
    
                    <ul style='margin-top: 8px; padding-left: 20px;'>
                        <li>Κωδικό Θέσης: <strong style='color: red;'>{positionRNG_Hashed}</strong></li>
                        <li>Τίτλο: <strong style='color: #1a73e8;'>{positionTitle}</strong></li>
                        <li>Στην εταιρεία: <strong>{companyName}</strong></li>
                    </ul>

                    <p>Η ενέργεια αυτή είναι οριστική, αλλά η Αίτησή σας θα συνεχίζει να εμφανίζεται στον Πίνακα Διαχείρισης των Αιτήσεών σας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send job withdrawal confirmation to student: {ex.Message}");
            throw;
        }
    }

    public async Task SendInternshipWithdrawalNotificationToCompany_AsStudent(
        string companyEmail,
        string companyName,
        string studentName,
        string studentSurname,
        string internshipTitle,
        string internshipRNG_Hashed)
        {
            try
            {
                using (var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    Timeout = 30000
                })
                {
                    var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                    var toAddress = new MailAddress(companyEmail, companyName);

                    string subject = $"Απόσυρση Αίτησης για την Πρακτική Άσκηση: {internshipTitle}";

                    string body = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                        <p>Αγαπητοί/ές στην {companyName},</p>

                        <p>Ο/Η φοιτητής/τρια <strong>{studentName} {studentSurname}</strong> έχει Αποσύρει την Αίτησή του/της για την Πρακτική Άσκηση:</p>
    
                        <ul style='margin-top: 8px; padding-left: 20px;'>
                            <li>Κωδικός Θέσης: <strong style='color: red;'>{internshipRNG_Hashed}</strong></li>
                            <li>Τίτλος: <strong style='color: #1a73e8;'>{internshipTitle}</strong></li>
                        </ul>

                        <p>Αυτή η ενέργεια είναι οριστική αλλά η Αίτηση του φοιτητή/ριας θα συνεχίσει να εμφανίζεται στον Πίνακα Διαχείρισής σας στις Θέσεις Πρακτικής Άσκησης.</p>

                        <p>Με εκτίμηση,<br>
                        <strong>AcadeMyHub</strong></p>

                        <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                            <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                            <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                        </div>
                    </div>";

                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                        ReplyToList = { new MailAddress(_supportEmail) }
                    })
                    {
                        await smtp.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send internship withdrawal notification to company: {ex.Message}");
                throw;
            }
    }

    public async Task SendInternshipWithdrawalConfirmationToStudent_AsCompany(
        string studentEmail,
        string studentName,
        string studentSurname,
        string internshipTitle,
        string internshipRNG_Hashed,
        string companyName)
        {
            try
            {
                using (var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    Timeout = 30000
                })
                {
                    var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                    var toAddress = new MailAddress(studentEmail, $"{studentName} {studentSurname}");

                    string subject = $"Επιτυχής Απόσυρση της Πρακτικής Άσκησης: {internshipTitle}";

                    string body = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                        <p>Αγαπητή/έ {studentName} {studentSurname},</p>

                        <p>Έχετε επιτυχώς Αποσύρει την Αίτησή σας για την Θέση Πρακτικής Άσκησης:</p>
    
                        <ul style='margin-top: 8px; padding-left: 20px;'>
                            <li>Κωδικός Θέσης: <strong style='color: red;'>{internshipRNG_Hashed}</strong></li>
                            <li>Τίτλος: <strong style='color: #1a73e8;'>{internshipTitle}</strong></li>
                            <li>Στην εταιρεία: <strong>{companyName}</strong></li>
                        </ul>

                        <p>Η ενέργεια αυτή είναι οριστική αλλά η Αίτησή σας θα συνεχίζει να εμφανίζεται στον Πίνακα Διαχείρισης των Αιτήσεών σας για τις Θέσεις Πρακτικής Άσκησης.</p>

                        <p>Με εκτίμηση,<br>
                        <strong>AcadeMyHub</strong></p>

                        <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                            <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                            <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                        </div>
                    </div>";

                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                        ReplyToList = { new MailAddress(_supportEmail) }
                    })
                    {
                        await smtp.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send internship withdrawal confirmation to student: {ex.Message}");
                throw;
            }
    }

    public async Task SendProfessorInternshipWithdrawalNotificationToProfessor(
        string professorEmail,
        string professorFullName,
        string studentName,
        string studentSurname,
        string internshipTitle,
        string internshipRNG_Hashed)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(professorEmail, professorFullName);

                string subject = $"Αποσύρθηκε Αίτηση για Πρακτική Άσκηση: {internshipTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητέ/ή Καθηγητά {professorFullName},</p>

                    <p>Ο/Η φοιτητής/τρια <strong>{studentName} {studentSurname}</strong> έχει Αποσύρει την Αίτησή του/της για την Πρακτική Άσκηση:</p>
    
                    <ul style='margin-top: 8px; padding-left: 20px;'>
                        <li>Κωδικός Θέσης: <strong style='color: red;'>{internshipRNG_Hashed}</strong></li>
                        <li>Τίτλος: <strong style='color: #1a73e8;'>{internshipTitle}</strong></li>
                    </ul>

                    <p>Αυτή η ενέργεια είναι οριστική αλλά η Αίτηση σας θα συνεχίζει να εμφανίζεται στον Πίνακα Διαχείρισης των Θέσεων Πρακτικής Άσκησής σας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send professor internship withdrawal notification: {ex.Message}");
            throw;
        }
    }

    public async Task SendProfessorInternshipWithdrawalConfirmationToStudent(
        string studentEmail,
        string studentName,
        string studentSurname,
        string internshipTitle,
        string internshipRNG_Hashed,
        string professorFullName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(studentEmail, $"{studentName} {studentSurname}");

                string subject = $"Επιτυχής Απόσυρση από την Πρακτική Άσκηση: {internshipTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητή/έ {studentName} {studentSurname},</p>

                    <p>Έχετε επιτυχώς Αποσύρει την Αίτησή σας για την Θέση Πρακτικής Άσκησης:</p>
    
                    <ul style='margin-top: 8px; padding-left: 20px;'>
                        <li>Κωδικός Θέσης: <strong style='color: red;'>{internshipRNG_Hashed}</strong></li>
                        <li>Τίτλος: <strong style='color: #1a73e8;'>{internshipTitle}</strong></li>
                        <li>Καθηγητή/τριας: <strong>{professorFullName}</strong></li>
                    </ul>

                    <p>Η ενέργειά σας είναι οριστική αλλά η Αίτησή σας θα συνέχίζει να εμφανίζεται στον Πίνακα Διαχείρισης των Αιτήσεών σας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send professor internship withdrawal confirmation: {ex.Message}");
            throw;
        }
    }

    public async Task SendProfessorThesisInterestNotificationToProfessor(
    string professorEmail,
    string professorFullName,
    string companyName,
    string hrName,
    string hrSurname,
    string hrEmail,
    string hrTelephone,
    string thesisTitle,
    string thesisRNG_Hashed)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(professorEmail, professorFullName);

                string subject = $"Νέο Ενδιαφέρον Εταιρείας για την Πτυχιακή Εργασία: {thesisTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητέ/ή Καθηγητά {professorFullName},</p>

                    <p>Η εταιρεία <strong>{companyName}</strong> έχει Εκφράσει Ενδιαφέρον για την Πτυχιακή Εργασία σας:</p>
    
                    <ul style='margin-top: 8px; padding-left: 20px;'>
                        <li>Κωδικός Θέσης: <strong style='color: red;'>{thesisRNG_Hashed}</strong></li>
                        <li>Τίτλος: <strong style='color: #1a73e8;'>{thesisTitle}</strong></li>
                    </ul>

                    <p>Στοιχεία επικοινωνίας εταιρείας:</p>
                    <ul style='margin-top: 8px; padding-left: 20px;'>
                        <li>Επωνυμία: {companyName}</li>
                        <li>Υπεύθυνος/η HR: {hrName} {hrSurname}</li>
                        <li>Email: {hrEmail}</li>
                        <li>Τηλέφωνο: {hrTelephone}</li>
                    </ul>

                    <p>Μπορείτε να δείτε περισσότερες λεπτομέρειες από τον πίνακα διαχείρισής σας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send professor thesis interest notification: {ex.Message}");
            throw;
        }
    }

    public async Task SendProfessorThesisInterestConfirmationToCompany(
        string companyEmail,
        string companyName,
        string hrName,
        string hrSurname,
        string thesisTitle,
        string thesisRNG_Hashed,
        string professorFullName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, $"{hrName} {hrSurname}");

                string subject = $"Επιτυχής Έκφραση Ενδιαφέροντος για Πτυχιακή: {thesisTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητοί/ές στην {companyName},</p>

                    <p>Έχετε Επιτυχώς Εκφράσει Ενδιαφέρον για την Πτυχιακή Εργασία:</p>
    
                    <ul style='margin-top: 8px; padding-left: 20px;'>
                        <li>Κωδικός Θέσης: <strong style='color: red;'>{thesisRNG_Hashed}</strong></li>
                        <li>Τίτλος: <strong style='color: #1a73e8;'>{thesisTitle}</strong></li>
                        <li>Καθηγητή/τριας: <strong>{professorFullName}</strong></li>
                    </ul>

                    <p>Ο/Η καθηγητής/τρια θα ενημερωθεί για το ενδιαφέρον σας και μπορεί να επικοινωνήσει μαζί σας για περαιτέρω συζητήσεις.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send professor thesis interest confirmation: {ex.Message}");
            throw;
        }
    }

    public async Task SendCompanyThesisInterestNotificationToCompany(
    string companyEmail,
    string companyName,
    string professorName,
    string professorSurname,
    string professorUniversity,
    string professorDepartment,
    string professorWorkTelephone,
    string professorPersonalTelephone,
    string professorPersonalWebsite,
    string thesisTitle,
    string thesisRNG_Hashed)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(companyEmail, companyName);

                string subject = $"Νέο Ενδιαφέρον Καθηγητή για Πτυχιακή: {thesisTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητοί/ές στην {companyName},</p>

                    <p>Ο/Η Καθηγητής/τρια <strong>{professorName} {professorSurname}</strong> έχει Εκφράσει Ενδιαφέρον για την Πτυχιακή Εργασία σας:</p>
    
                    <ul style='margin-top: 8px; padding-left: 20px;'>
                        <li>Κωδικός Θέσης: <strong style='color: red;'>{thesisRNG_Hashed}</strong></li>
                        <li>Τίτλος: <strong style='color: #1a73e8;'>{thesisTitle}</strong></li>
                    </ul>

                    <p>Στοιχεία επικοινωνίας καθηγητή:</p>
                    <ul style='margin-top: 8px; padding-left: 20px;'>
                        <li>Πανεπιστήμιο: {professorUniversity}</li>
                        <li>Τμήμα: {professorDepartment}</li>
                        <li>Τηλέφωνο Εργασίας: {professorWorkTelephone}</li>
                        <li>Προσωπικό Τηλέφωνο: {professorPersonalTelephone}</li>
                        <li>Ιστότοπος: <a href='{professorPersonalWebsite}'>{professorPersonalWebsite}</a></li>
                    </ul>

                    <p>Μπορείτε να δείτε περισσότερες λεπτομέρειες από τον Πίνακα Διαχείρισής σας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send company thesis interest notification: {ex.Message}");
            throw;
        }
    }

    public async Task SendCompanyThesisInterestConfirmationToProfessor(
        string professorEmail,
        string professorFullName,
        string thesisTitle,
        string thesisRNG_Hashed,
        string companyName)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(professorEmail, professorFullName);

                string subject = $"Επιτυχής Έκφραση Ενδιαφέροντος για Πτυχιακή: {thesisTitle}";

                string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητέ/ή {professorFullName},</p>

                    <p>Έχετε Επιτυχώς Εκφράσει Eνδιαφέρον για την Πτυχιακή Εργασία:</p>
    
                    <ul style='margin-top: 8px; padding-left: 20px;'>
                        <li>Κωδικός Θέσης: <strong style='color: red;'>{thesisRNG_Hashed}</strong></li>
                        <li>Τίτλος: <strong style='color: #1a73e8;'>{thesisTitle}</strong></li>
                        <li>Εταιρεία: <strong>{companyName}</strong></li>
                    </ul>

                    <p>Η Εταιρεία θα ενημερωθεί για το ενδιαφέρον σας και μπορεί να επικοινωνήσει μαζί σας για περαιτέρω συζητήσεις.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send company thesis interest confirmation: {ex.Message}");
            throw;
        }
    }

    public async Task SendInvitationEmailToProfessorToRegisterOnPlatform(string email)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(email);

                string subject = "Πρόσκληση για εγγραφή στο AcadeMyHub";

                string body = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <p>Αγαπητέ/ή συνάδελφε,</p>

            <p>Έχετε λάβει πρόσκληση να εγγραφείτε στην πλατφόρμα AcadeMyHub ως Καθηγητής.</p>

            <p>Για να ολοκληρώσετε την εγγραφή σας, παρακαλώ επισκεφθείτε τον ακόλουθο σύνδεσμο:</p>
            
            <p><a href='https://academyhub.gr' style='background-color: #1a73e8; color: white; padding: 10px 15px; text-decoration: none; border-radius: 4px;'>Εγγραφή στο AcadeMyHub</a></p>

            <p>Με εκτίμηση,<br>
            <strong>AcadeMyHub</strong></p>

            <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
            </div>
        </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send professor invitation: {ex.Message}");
            throw;
        }
    }

    public async Task SendInvitationEmailToStudentToRegisterOnPlatform(string email)
    {
        try
        {
            using (var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                Timeout = 30000
            })
            {
                var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
                var toAddress = new MailAddress(email);

                string subject = "Πρόσκληση για εγγραφή στο AcadeMyHub ως Φοιτητής";

                string body = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <p>Γεια σας,</p>

            <p>Έχετε λάβει πρόσκληση να εγγραφείτε στην πλατφόρμα AcadeMyHub ως Φοιτητής/Μέλος.</p>

            <p>Για να ολοκληρώσετε την εγγραφή σας, παρακαλώ επισκεφθείτε τον ακόλουθο σύνδεσμο:</p>
            
            <p><a href='https://your-academyhub-url.com/register/student' style='background-color: #1a73e8; color: white; padding: 10px 15px; text-decoration: none; border-radius: 4px;'>Εγγραφή στο AcadeMyHub</a></p>

            <p>Με εκτίμηση,<br>
            <strong>AcadeMyHub</strong></p>

            <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
            </div>
        </div>";

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(_supportEmail) }
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send student invitation: {ex.Message}");
            throw;
        }
    }

	public async Task SendConfirmationToCompanyForInterestInProfessorEvent(
	string companyEmail,
	string companyName,
	string eventTitle,
	string professorName,
	string professorSurname,
	string eventRNG_Hashed,
	int numberOfPeople)
	{
		try
		{
			using (var smtp = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
				Timeout = 30000
			})
			{
				var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
				var toAddress = new MailAddress(companyEmail, companyName);

				string subject = $"Επιτυχής Έκφραση Ενδιαφέροντος για Εκδήλωση: {eventTitle}";

				string body = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <p>Αγαπητοί/ές {companyName},</p>

            <p>Η έκφραση ενδιαφέροντος της εταιρείας σας για την εκδήλωση με Κωδικό: 
                <strong style='color: red;'>{eventRNG_Hashed}</strong> και Τίτλο: 
                <strong style='color: #1a73e8;'>{eventTitle}</strong>, του καθηγητή: 
                <strong>{professorName} {professorSurname}</strong>, 
                έχει καταχωρηθεί <strong style='color: #34a853;'>Επιτυχώς</strong>!</p>

            <p>Στοιχεία δήλωσης:</p>
            <ul style='margin-top: 8px; padding-left: 20px;'>
                <li>Αριθμός ατόμων: <strong>{numberOfPeople}</strong></li>
            </ul>

            <p>Ο καθηγητής θα επανεξετάσει το ενδιαφέρον σας και θα ενημερωθείτε για την εξέλιξή της.</p>

            <p>Με εκτίμηση,<br>
            <strong>AcadeMyHub</strong></p>

            <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
            </div>
        </div>";

				using (var message = new MailMessage(fromAddress, toAddress)
				{
					Subject = subject,
					Body = body,
					IsBodyHtml = true,
					ReplyToList = { new MailAddress(_supportEmail) }
				})
				{
					await smtp.SendMailAsync(message);
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to send event interest confirmation to company: {ex.Message}");
			throw;
		}
	}

	public async Task SendNotificationToProfessorForCompanyInterestInEvent(
		string professorEmail,
		string professorName,
		string professorSurname,
		string companyName,
		string companyEmail,
		string companyTelephone,
		string eventTitle,
		string eventRNG_Hashed,
		int numberOfPeople)
	{
		try
		{
			using (var smtp = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
				Timeout = 30000
			})
			{
				var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
				var toAddress = new MailAddress(professorEmail, $"{professorName} {professorSurname}");

				string subject = $"Νέα Έκφραση Ενδιαφέροντος για Εκδήλωση: {eventTitle}";

				string body = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <p>Αγαπητέ/ή {professorName} {professorSurname},</p>

            <p>Έχετε λάβει μια νέα έκφραση ενδιαφέροντος για την εκδήλωση με Κωδικό: 
                <strong style='color: red;'>{eventRNG_Hashed}</strong> και Τίτλο: 
                <strong style='color: #1a73e8;'>{eventTitle}</strong>, από την εταιρεία: 
                <strong>{companyName}</strong>.</p>

            <p>Στοιχεία εταιρείας:</p>
            <ul style='margin-top: 8px; padding-left: 20px;'>
                <li>Επωνυμία: {companyName}</li>
                <li>Email: {companyEmail}</li>
                <li>Τηλέφωνο: {companyTelephone}</li>
                <li>Αριθμός ατόμων: {numberOfPeople}</li>
            </ul>

            <p>Μπορείτε να δείτε όλες τις εκφράσεις ενδιαφέροντος από τον πίνακα διαχείρισης σας.</p>

            <p>Με εκτίμηση,<br>
            <strong>AcadeMyHub</strong></p>

            <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
            </div>
        </div>";

				using (var message = new MailMessage(fromAddress, toAddress)
				{
					Subject = subject,
					Body = body,
					IsBodyHtml = true,
					ReplyToList = { new MailAddress(_supportEmail) }
				})
				{
					await smtp.SendMailAsync(message);
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to send event interest notification to professor: {ex.Message}");
			throw;
		}
	}

	public async Task SendConfirmationToProfessorForInterestInCompanyEvent(
	string professorEmail,
	string professorName,
	string professorSurname,
	string eventTitle,
	string companyName,
	string eventRNG_Hashed)
	{
		try
		{
			using (var smtp = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
				Timeout = 30000
			})
			{
				var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
				var toAddress = new MailAddress(professorEmail, $"{professorName} {professorSurname}");

				string subject = $"Επιτυχής Έκφραση Ενδιαφέροντος για Εκδήλωση: {eventTitle}";

				string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητέ/ή {professorName} {professorSurname},</p>

                    <p>Η έκφραση ενδιαφέροντος σας για την εκδήλωση με Κωδικό: 
                        <strong style='color: red;'>{eventRNG_Hashed}</strong> και Τίτλο: 
                        <strong style='color: #1a73e8;'>{eventTitle}</strong>, της εταιρείας: 
                        <strong>{companyName}</strong>, 
                        έχει καταχωρηθεί <strong style='color: #34a853;'>Επιτυχώς</strong>!</p>

                    <p>Η εταιρεία θα επανεξετάσει το ενδιαφέρον σας και θα ενημερωθείτε για την εξέλιξή της.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

				using (var message = new MailMessage(fromAddress, toAddress)
				{
					Subject = subject,
					Body = body,
					IsBodyHtml = true,
					ReplyToList = { new MailAddress(_supportEmail) }
				})
				{
					await smtp.SendMailAsync(message);
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to send event interest confirmation to professor: {ex.Message}");
			throw;
		}
	}

	public async Task SendNotificationToCompanyForProfessorInterestInEvent(
		string companyEmail,
		string companyName,
		string professorName,
		string professorSurname,
		string professorEmail,
		string professorTelephone,
		string eventTitle,
		string eventRNG_Hashed)
	{
		try
		{
			using (var smtp = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
				Timeout = 30000
			})
			{
				var fromAddress = new MailAddress(_noReplyEmail, "AcadeMyHub");
				var toAddress = new MailAddress(companyEmail, companyName);

				string subject = $"Νέα Έκφραση Ενδιαφέροντος για Εκδήλωση: {eventTitle}";

				string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <p>Αγαπητοί/ές {companyName},</p>

                    <p>Έχετε λάβει μια νέα έκφραση ενδιαφέροντος για την εκδήλωση με Κωδικό: 
                        <strong style='color: red;'>{eventRNG_Hashed}</strong> και Τίτλο: 
                        <strong style='color: #1a73e8;'>{eventTitle}</strong>, από τον/την καθηγητή: 
                        <strong>{professorName} {professorSurname}</strong>.</p>

                    <p>Στοιχεία καθηγητή:</p>
                    <ul style='margin-top: 8px; padding-left: 20px;'>
                        <li>Ονοματεπώνυμο: {professorName} {professorSurname}</li>
                        <li>Email: {professorEmail}</li>
                        <li>Τηλέφωνο: {professorTelephone}</li>
                    </ul>

                    <p>Μπορείτε να δείτε όλες τις εκφράσεις ενδιαφέροντος από τον πίνακα διαχείρισης σας.</p>

                    <p>Με εκτίμηση,<br>
                    <strong>AcadeMyHub</strong></p>

                    <div style='margin-top: 20px; font-size: 0.9em; color: #5f6368;'>
                        <p>Για οποιαδήποτε απορία, παρακαλώ επικοινωνήστε στο: {_supportEmail}</p>
                        <p style='font-style: italic;'>Αυτό είναι ένα αυτόματο μήνυμα. Παρακαλώ μην απαντάτε σε αυτό το email.</p>
                    </div>
                </div>";

				using (var message = new MailMessage(fromAddress, toAddress)
				{
					Subject = subject,
					Body = body,
					IsBodyHtml = true,
					ReplyToList = { new MailAddress(_supportEmail) }
				})
				{
					await smtp.SendMailAsync(message);
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to send event interest notification to company: {ex.Message}");
			throw;
		}
	}

}

