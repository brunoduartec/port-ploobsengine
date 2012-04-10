﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PloobsEngine.IA
{
    public class BlackBoard
    {
        private Dictionary<String, object> intelligence = new Dictionary<string, object>();
        private Dictionary<String, List<Trigger>> triggers = new Dictionary<string, List<Trigger>>();
        private Dictionary<String, Queue<ITask>> TaskQueue = new Dictionary<string, Queue<ITask>>();

        public void AddTaskToQueue(ITask Task)
        {
            if (TaskQueue.ContainsKey(Task.QueueNameHandler))
            {
                TaskQueue[Task.QueueNameHandler].Enqueue(Task);
            }
            else
            {
                TaskQueue[Task.QueueNameHandler] = new Queue<ITask>();
                TaskQueue[Task.QueueNameHandler].Enqueue(Task);
            }
        }

        public ITask DeQueueTaskFrom(String name)
        {
            if(TaskQueue.ContainsKey(name) && TaskQueue[name].Count >0)
                return TaskQueue[name].Dequeue();
            return null;
        }

        
        public void SetEntry<T>(String name, T intel)
        {
            TriggerState TriggerState = intelligence.ContainsKey(name) == true ? TriggerState.VALUE_CHANGED : TriggerState.VALUE_ADDED;
            intelligence[name] = intel;
            if (triggers.ContainsKey(name))
            {
                foreach (var item in triggers[name])
                {
                    item.Fire(TriggerState);
                }
            }
        }

        public void RemoveEntry(String name)
        {
            intelligence.Remove(name);
            if (triggers.ContainsKey(name))
            {
                foreach (var item in triggers[name])
                {
                    item.Fire(TriggerState.VALUE_REMOVED);
                }
            }
        }

        public T GetEntry<T>(String name)
        {
            if (intelligence.ContainsKey(name))
                return (T)intelligence[name];
            return default(T);
        }

        public object GetEntry(String name)
        {
            if (intelligence.ContainsKey(name))
                return intelligence[name];
            return null;
        }

        public void AddTrigger(Trigger Trigger, bool evaluateNow = false)
        {
            Trigger.BlackBoard = this;
            foreach (var IntellName in Trigger.WorldPropertiesMonitored)
            {

                if (triggers.ContainsKey(IntellName))
                {
                    triggers[IntellName].Add(Trigger);
                }
                else
                {
                    triggers[IntellName] = new List<Trigger>();
                    triggers[IntellName].Add(Trigger);
                }
            }
            if (evaluateNow)
            {
                Trigger.Fire(TriggerState.TRIGGER_ADDED);
            }
        }

        public void RemoveTrigger(Trigger Trigger)
        {
            foreach (var IntellName in Trigger.WorldPropertiesMonitored)
            {
                triggers[IntellName].Remove(Trigger);
            }
        }

        public List<Trigger> TriggerList(String Name)
        {
            return triggers[Name];
        }
    }
}