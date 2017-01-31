using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

public class TeeMS_SignalR : Hub
{
    public void Hello()
    {
        Clients.All.hello();
    }        
}
