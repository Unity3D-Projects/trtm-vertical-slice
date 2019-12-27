
// Примечание. Для запуска созданного кода может потребоваться NET Framework версии 4.5 или более поздней версии и .NET Core или Standard версии 2.0 или более поздней.
using System.Collections.Generic;
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class GameState
{

    private saveLog logField;

    private saveExecute executeField;

    private saveState[] statesField;

    private object varsField;

    /// <remarks/>
    public saveLog log
    {
        get
        {
            return this.logField;
        }
        set
        {
            this.logField = value;
        }
    }

    /// <remarks/>
    public saveExecute execute
    {
        get
        {
            return this.executeField;
        }
        set
        {
            this.executeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("state", IsNullable = false)]
    public saveState[] states
    {
        get
        {
            return this.statesField;
        }
        set
        {
            this.statesField = value;
        }
    }

    /// <remarks/>
    public object vars
    {
        get
        {
            return this.varsField;
        }
        set
        {
            this.varsField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class saveLog
{

    private LinkedList<object> itemsField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("buttonGroup", typeof(saveLogButtonGroup))]
    [System.Xml.Serialization.XmlElementAttribute("endGame", typeof(saveLogEndGame))]
    [System.Xml.Serialization.XmlElementAttribute("phrase", typeof(string))]
    public LinkedList<object> Items
    {
        get
        {
            return this.itemsField;
        }
        set
        {
            this.itemsField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class saveLogButtonGroup
{

    private saveLogButtonGroupButton[] buttonField;

    private string idField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("button")]
    public saveLogButtonGroupButton[] button
    {
        get
        {
            return this.buttonField;
        }
        set
        {
            this.buttonField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class saveLogButtonGroupButton
{

    private bool pressedField;

    private string valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool pressed
    {
        get
        {
            return this.pressedField;
        }
        set
        {
            this.pressedField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class saveLogEndGame
{
    public saveLogEndGame(bool win)
    {
        this.win = win;
    }
    private bool winField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool win
    {
        get
        {
            return this.winField;
        }
        set
        {
            this.winField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class saveExecute
{

    private string tnameField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string tname
    {
        get
        {
            return this.tnameField;
        }
        set
        {
            this.tnameField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class saveState
{

    private string idField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }
}

