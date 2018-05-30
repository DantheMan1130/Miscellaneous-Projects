using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class task
{
    public string name;
    public int deadline;
    public int staticDeadline;
    public int ET1188;
    public int staticET1188;
    public int ET918;
    public int ET648;
    public int ET384;
    public int taskDone;

    public task(string a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9)
    {
        name = a1;
        deadline = a2;
        staticDeadline = a3;
        ET1188 = a4;
        staticET1188 = a5;
        ET918 = a6;
        ET648 = a7;
        ET384 = a8;
        taskDone = a9;
    }
}

namespace HW3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string file = File.ReadAllText(args[0]);
                string[] strings = file.Split(new char[] {' ', '\n'}, StringSplitOptions.RemoveEmptyEntries);

                int length = strings.Length;
                int numberofTasks = int.Parse(strings[0]);
                int totalExecutionTime = int.Parse(strings[1]);
                int AP1188 = int.Parse(strings[2]);
                int AP918 = int.Parse(strings[3]);
                int AP648 = int.Parse(strings[4]);
                int AP384 = int.Parse(strings[5]);
                int idlePower = int.Parse(strings[6]);

                // Earliest deadline first.
                if (args[1] == "EDF" && args[2] == "null") // Can we use null?
                {
                    // Tasks
                    task task1 = new global::task(strings[7], int.Parse(strings[8]), int.Parse(strings[8]), int.Parse(strings[9]), int.Parse(strings[9]), int.Parse(strings[10]), int.Parse(strings[11]), int.Parse(strings[12]), 0);
                    task task2 = new global::task(strings[13], int.Parse(strings[14]), int.Parse(strings[14]), int.Parse(strings[15]), int.Parse(strings[15]), int.Parse(strings[16]), int.Parse(strings[17]), int.Parse(strings[18]), 0);
                    task task3 = new global::task(strings[19], int.Parse(strings[20]), int.Parse(strings[20]), int.Parse(strings[21]), int.Parse(strings[21]), int.Parse(strings[22]), int.Parse(strings[23]), int.Parse(strings[24]), 0);
                    task task4 = new global::task(strings[25], int.Parse(strings[26]), int.Parse(strings[26]), int.Parse(strings[27]), int.Parse(strings[27]), int.Parse(strings[28]), int.Parse(strings[29]), int.Parse(strings[30]), 0);
                    task task5 = new global::task(strings[31], int.Parse(strings[32]), int.Parse(strings[32]), int.Parse(strings[33]), int.Parse(strings[33]), int.Parse(strings[34]), int.Parse(strings[35]), int.Parse(strings[36]), 0);

                    List<task> taskList = new List<task>();
                    taskList.Add(task1);
                    taskList.Add(task2);
                    taskList.Add(task3);
                    taskList.Add(task4);
                    taskList.Add(task5);

                    // Miscellaneous variables.
                    string scheduledTask = null;
                    string lastScheduledTask = null;
                    int lastDeadline = totalExecutionTime * 2;
                    int currentTime;

                    int startTime = 0;
                    int endTime = 0;
                    int executed = 0;
                    int endFlag = 0;
                    string taskFrequency = "1188";
                    float powerCalc = 0;
                    int powerVal = AP1188;
                    int IDLEtime = 0;
                    int idleCount = 0;

                    FileStream filestream = new FileStream("outputEDF.txt", FileMode.Create);
                    var streamwriter = new StreamWriter(filestream);
                    streamwriter.AutoFlush = true;
                    Console.SetOut(streamwriter);
                    Console.SetError(streamwriter);

                    for (currentTime = 0; currentTime < totalExecutionTime; ++currentTime)
                    {
                        foreach (task tasks in taskList)
                        {
                            int currentDeadline = tasks.deadline;
                            if (currentDeadline < lastDeadline && (currentTime >= (tasks.deadline - tasks.staticDeadline)))
                            {
                                scheduledTask = tasks.name;
                                lastDeadline = tasks.deadline;
                                tasks.taskDone = 0;
                                idleCount = 0;
                            }
                        }
                        lastDeadline = totalExecutionTime * 2;

                        foreach (task tasks in taskList)
                        {
                            if (tasks.name == scheduledTask && tasks.taskDone == 0) // Will need to be modified for EE.
                            {
                                tasks.ET1188 = tasks.ET1188 - 1;
                            }

                            if (tasks.taskDone == 1)
                            {
                                executed = executed + 1;
                                if (executed == numberofTasks)
                                {
                                    scheduledTask = "IDLE";
                                    idleCount = idleCount + 1;
                                    if (idleCount == 1)
                                    {
                                        taskFrequency = taskFrequency;
                                    }

                                    else
                                    {
                                        taskFrequency = "IDLE";
                                    }
                                    powerVal = idlePower;
                                }
                            }

                            if (tasks.ET1188 == 0)
                            {
                                tasks.ET1188 = tasks.staticET1188;
                                tasks.taskDone = 1;
                                tasks.deadline = tasks.deadline + tasks.staticDeadline;
                            }

                            if ((lastScheduledTask != scheduledTask && (currentTime != 0)) || (currentTime == totalExecutionTime - 1))
                            {
                                if (currentTime == totalExecutionTime - 1 && endFlag == 0) // If end of execution is reached.
                                {
                                    endTime = currentTime - startTime;
                                    powerCalc = (((currentTime - startTime) * powerVal));
                                    Console.WriteLine("{0} {1} {2} {3} {4}x10^-3J", startTime, lastScheduledTask, taskFrequency, endTime, powerCalc);
                                    if (lastScheduledTask == "IDLE")
                                    {
                                        IDLEtime = IDLEtime + endTime;
                                    }
                                    endFlag = 1;
                                }

                                else if (endFlag == 0 && (scheduledTask != "IDLE" || idleCount == 1)) // As long as scheduled task isn't "IDLE".
                                {
                                    endTime = currentTime - startTime;
                                    powerCalc = (((currentTime - startTime) * powerVal));
                                    Console.WriteLine("{0} {1} {2} {3} {4}x10^-3J", startTime, lastScheduledTask, taskFrequency, endTime, powerCalc);
                                    if (lastScheduledTask == "IDLE")
                                    {
                                        IDLEtime = IDLEtime + endTime;
                                    }
                                    startTime = currentTime;
                                    taskFrequency = "1188";
                                    powerVal = AP1188;
                                }
                            }
                            lastScheduledTask = scheduledTask;
                        }
                        executed = 0;
                    }
                    double energyConsumption = ((AP1188 * (totalExecutionTime - IDLEtime)) + (idlePower * IDLEtime));
                    Console.WriteLine("Total energy consumption: {0}x10^-3J", energyConsumption);
                    Console.WriteLine("Percentage of time spent idle: {0}x10^-1%", IDLEtime);
                    Console.WriteLine("Total execution time: {0}", totalExecutionTime);
                }

                // Rate monotonic.
                if (args[1] == "RM" && args[2] == "null")
                {
                    // Tasks
                    task task1 = new global::task(strings[7], int.Parse(strings[8]), int.Parse(strings[8]), int.Parse(strings[9]), int.Parse(strings[9]), int.Parse(strings[10]), int.Parse(strings[11]), int.Parse(strings[12]), 0);
                    task task2 = new global::task(strings[13], int.Parse(strings[14]), int.Parse(strings[14]), int.Parse(strings[15]), int.Parse(strings[15]), int.Parse(strings[16]), int.Parse(strings[17]), int.Parse(strings[18]), 0);
                    task task3 = new global::task(strings[19], int.Parse(strings[20]), int.Parse(strings[20]), int.Parse(strings[21]), int.Parse(strings[21]), int.Parse(strings[22]), int.Parse(strings[23]), int.Parse(strings[24]), 0);
                    task task4 = new global::task(strings[25], int.Parse(strings[26]), int.Parse(strings[26]), int.Parse(strings[27]), int.Parse(strings[27]), int.Parse(strings[28]), int.Parse(strings[29]), int.Parse(strings[30]), 0);
                    task task5 = new global::task(strings[31], int.Parse(strings[32]), int.Parse(strings[32]), int.Parse(strings[33]), int.Parse(strings[33]), int.Parse(strings[34]), int.Parse(strings[35]), int.Parse(strings[36]), 0);

                    List<task> taskList = new List<task>();
                    taskList.Add(task1);
                    taskList.Add(task2);
                    taskList.Add(task3);
                    taskList.Add(task4);
                    taskList.Add(task5);

                    // Miscellaneous variables.
                    string scheduledTask = null;
                    string lastScheduledTask = null;
                    int lastDeadline = totalExecutionTime * 2;
                    int currentTime;

                    int startTime = 0;
                    int endTime = 0;
                    int executed = 0;
                    int endFlag = 0;
                    string taskFrequency = "1188";
                    float powerCalc = 0;
                    int powerVal = AP1188;
                    int IDLEtime = 0;
                    int idleCount = 0;

                    FileStream filestream = new FileStream("outputRM.txt", FileMode.Create);
                    var streamwriter = new StreamWriter(filestream);
                    streamwriter.AutoFlush = true;
                    Console.SetOut(streamwriter);
                    Console.SetError(streamwriter);

                    for (currentTime = 0; currentTime < totalExecutionTime; ++currentTime)
                    {
                        foreach (task tasks in taskList)
                        {
                            int currentDeadline = tasks.deadline;
                            if (currentDeadline < lastDeadline && (currentTime >= (tasks.staticDeadline - tasks.deadline)))
                            {
                                scheduledTask = tasks.name;
                                lastDeadline = tasks.deadline;
                                tasks.taskDone = 0;
                                idleCount = 0;
                            }
                        }
                        lastDeadline = totalExecutionTime * 2;

                        foreach (task tasks in taskList)
                        {
                            if (tasks.name == scheduledTask && tasks.taskDone == 0)
                            {
                                tasks.ET1188 = tasks.ET1188 - 1;
                            }

                            if (tasks.taskDone == 1)
                            {
                                executed = executed + 1;
                                if (executed == numberofTasks)
                                {
                                    scheduledTask = "IDLE";
                                    idleCount = idleCount + 1;
                                    if (idleCount == 1)
                                    {
                                        taskFrequency = taskFrequency;
                                    }

                                    else
                                    {
                                        taskFrequency = "IDLE";
                                    }
                                    powerVal = idlePower;
                                }
                            }

                            if (tasks.ET1188 == 0)
                            {
                                tasks.ET1188 = tasks.staticET1188;
                                tasks.taskDone = 1;
                                tasks.staticDeadline = tasks.deadline + tasks.staticDeadline;
                            }

                            if ((lastScheduledTask != scheduledTask && (currentTime != 0)) || (currentTime == totalExecutionTime - 1))
                            {
                                if (currentTime == totalExecutionTime - 1 && endFlag == 0) // If end of execution is reached.
                                {
                                    endTime = currentTime - startTime;
                                    powerCalc = (((currentTime - startTime) * powerVal));
                                    Console.WriteLine("{0} {1} {2} {3} {4}x10^-3J", startTime, lastScheduledTask, taskFrequency, endTime, powerCalc);
                                    if (lastScheduledTask == "IDLE")
                                    {
                                        IDLEtime = IDLEtime + endTime;
                                    }
                                    endFlag = 1;
                                }

                                else if (endFlag == 0 && (scheduledTask != "IDLE" || idleCount == 0)) // As long as scheduled task isn't "IDLE".
                                {
                                    endTime = currentTime - startTime;
                                    powerCalc = (((currentTime - startTime) * powerVal));
                                    Console.WriteLine("{0} {1} {2} {3} {4}x10^-3J", startTime, lastScheduledTask, taskFrequency, endTime, powerCalc);
                                    if (lastScheduledTask == "IDLE")
                                    {
                                        IDLEtime = IDLEtime + endTime;
                                    }
                                    startTime = currentTime;
                                    taskFrequency = "1188";
                                    powerVal = AP1188;
                                }
                            }
                            lastScheduledTask = scheduledTask;
                        }
                        executed = 0;
                    }
                    double energyConsumption = ((AP1188 * (totalExecutionTime - IDLEtime)) + (idlePower * IDLEtime));
                    Console.WriteLine("Total energy consumption: {0}x10^-3J", energyConsumption);
                    Console.WriteLine("Percentage of time spent idle: {0}x10^-1%", IDLEtime);
                    Console.WriteLine("Total execution time: {0}", totalExecutionTime);
                }

                // Energy-efficient earliest deadline first (EE).
                if (args[1] == "EDF" && args[2] == "EE")
                {
                    // Tasks
                    task task1 = new global::task(strings[7], int.Parse(strings[8]), int.Parse(strings[8]), int.Parse(strings[9]), int.Parse(strings[9]), int.Parse(strings[10]), int.Parse(strings[11]), int.Parse(strings[12]), 0);
                    task task2 = new global::task(strings[13], int.Parse(strings[14]), int.Parse(strings[14]), int.Parse(strings[15]), int.Parse(strings[15]), int.Parse(strings[16]), int.Parse(strings[17]), int.Parse(strings[18]), 0);
                    task task3 = new global::task(strings[19], int.Parse(strings[20]), int.Parse(strings[20]), int.Parse(strings[21]), int.Parse(strings[21]), int.Parse(strings[22]), int.Parse(strings[23]), int.Parse(strings[24]), 0);
                    task task4 = new global::task(strings[25], int.Parse(strings[26]), int.Parse(strings[26]), int.Parse(strings[27]), int.Parse(strings[27]), int.Parse(strings[28]), int.Parse(strings[29]), int.Parse(strings[30]), 0);
                    task task5 = new global::task(strings[31], int.Parse(strings[32]), int.Parse(strings[32]), int.Parse(strings[33]), int.Parse(strings[33]), int.Parse(strings[34]), int.Parse(strings[35]), int.Parse(strings[36]), 0);

                    List<task> taskList = new List<task>();
                    taskList.Add(task1);
                    taskList.Add(task2);
                    taskList.Add(task3);
                    taskList.Add(task4);
                    taskList.Add(task5);

                    // Miscellaneous variables.
                    string scheduledTask = null;
                    string lastScheduledTask = null;
                    int lastDeadline = totalExecutionTime * 2;
                    int currentTime;

                    int startTime = 0;
                    int endTime = 0;
                    int executed = 0;
                    int endFlag = 0;
                    string taskFrequency = "1188";
                    float powerCalc = 0;
                    int powerVal = AP1188;
                    int IDLEtime = 0;
                    int idleCount = 0;

                    FileStream filestream = new FileStream("outputEDF_EE.txt", FileMode.Create);
                    var streamwriter = new StreamWriter(filestream);
                    streamwriter.AutoFlush = true;
                    Console.SetOut(streamwriter);
                    Console.SetError(streamwriter);

                    for (currentTime = 0; currentTime < totalExecutionTime; ++currentTime)
                    {
                        foreach (task tasks in taskList)
                        {
                            int currentDeadline = tasks.deadline;
                            if (currentDeadline < lastDeadline && (currentTime >= (tasks.deadline - tasks.staticDeadline)))
                            {
                                scheduledTask = tasks.name;
                                lastDeadline = tasks.deadline;
                                tasks.taskDone = 0;
                                idleCount = 0;
                            }
                        }
                        lastDeadline = totalExecutionTime * 2;

                        foreach (task tasks in taskList)
                        {
                            if (tasks.name == scheduledTask && tasks.taskDone == 0) // Will need to be modified for EE.
                            {
                                tasks.ET1188 = tasks.ET1188 - 1;
                            }

                            if (tasks.taskDone == 1)
                            {
                                executed = executed + 1;
                                if (executed == numberofTasks)
                                {
                                    scheduledTask = "IDLE";
                                    idleCount = idleCount + 1;
                                    if (idleCount == 1)
                                    {
                                        taskFrequency = taskFrequency;
                                    }

                                    else
                                    {
                                        taskFrequency = "IDLE";
                                    }
                                    powerVal = idlePower;
                                }
                            }

                            if (tasks.ET1188 == 0)
                            {
                                tasks.ET1188 = tasks.staticET1188;
                                tasks.taskDone = 1;
                                tasks.deadline = tasks.deadline + tasks.staticDeadline;
                            }

                            if ((lastScheduledTask != scheduledTask && (currentTime != 0)) || (currentTime == totalExecutionTime - 1))
                            {
                                if (currentTime == totalExecutionTime - 1 && endFlag == 0) // If end of execution is reached.
                                {
                                    endTime = currentTime - startTime;
                                    powerCalc = (((currentTime - startTime) * powerVal));
                                    Console.WriteLine("{0} {1} {2} {3} {4}x10^-3J", startTime, lastScheduledTask, taskFrequency, endTime, powerCalc);
                                    if (lastScheduledTask == "IDLE")
                                    {
                                        IDLEtime = IDLEtime + endTime;
                                    }
                                    endFlag = 1;
                                }

                                else if (endFlag == 0 && (scheduledTask != "IDLE" || idleCount == 0)) // As long as scheduled task isn't "IDLE".
                                {
                                    endTime = currentTime - startTime;
                                    powerCalc = (((currentTime - startTime) * powerVal));
                                    Console.WriteLine("{0} {1} {2} {3} {4}x10^-3J", startTime, lastScheduledTask, taskFrequency, endTime, powerCalc);
                                    if (lastScheduledTask == "IDLE")
                                    {
                                        IDLEtime = IDLEtime + endTime;
                                    }
                                    startTime = currentTime;
                                    taskFrequency = "1188";
                                    powerVal = AP1188;
                                }
                            }
                            lastScheduledTask = scheduledTask;
                        }
                        executed = 0;
                    }
                    double energyConsumption = ((AP1188 * (totalExecutionTime - IDLEtime)) + (idlePower * IDLEtime));
                    Console.WriteLine("Total energy consumption: {0}x10^-3J", energyConsumption);
                    Console.WriteLine("Percentage of time spent idle: {0}x10^-1%", IDLEtime);
                    Console.WriteLine("Total execution time: {0}", totalExecutionTime);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

        }
    }
}
