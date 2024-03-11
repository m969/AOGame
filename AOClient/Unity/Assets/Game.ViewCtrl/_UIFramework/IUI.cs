using System;

public interface IUI
{
    void OnCreate(object uiObj);
    void OnOpen();
    void OnClose();
}