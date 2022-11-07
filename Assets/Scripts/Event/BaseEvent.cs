using System.Collections;
using System.Collections.Generic;


public class BaseEvent 
{
    protected Hashtable arguments;
    protected EventType type;
    protected object sender;
    public EventType Type
    {
        get
        {
            return this.type;
        }
        set
        {
            this.type = value;
        }
    }
    public IDictionary Params
    {
        get
        {
            return this.arguments;
        }
        set
        {
            this.arguments=(value as Hashtable);
        }
    }
    public object Sender
    {
        get
        {
            return this.sender;
        }
        set
        {
            this.sender = value;
        }
    }
    public override string ToString()
    {
        return this.type + "[" + ((this.sender == null) ? "null" : this.sender.ToString()) + "]";
    }
    public BaseEvent clone()
    {
        return new BaseEvent(this.type, this.arguments, sender);
    }
    public BaseEvent(EventType type,object sender)
    {
        this.Type = type;
        this.Sender = sender;
        if (this.arguments == null)
        {
            this.arguments = new Hashtable();
        }
    }
    public BaseEvent(EventType type,Hashtable args, object sender)
    {
        this.Type = type;
        this.arguments = args;
        Sender = sender;
        if (this.arguments == null)
        {
            this.arguments = new Hashtable();
        }
    }
}
