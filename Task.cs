          public int Start(){            
            Task t = new Task(()=> 
            {
            System.Threading.Thread.Sleep(10000);  
                                          Run();
            });
             t.Start();  
  
            //Wait for 1 second  
            bool rValue = t.Wait(1000);  
            Console.WriteLine("Main Process Finished");
            return 0;
        }
       
       public Task Run()
        {
            System.Console.WriteLine("Printing TAsk Stareted");
            for (int i = 0; i < 10000; i++)
            {
                System.Console.WriteLine(i);
            }
            return Task.CompletedTask;
        }
