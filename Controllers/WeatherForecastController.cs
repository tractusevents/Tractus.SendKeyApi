using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Tractus.SendKeyApi.Controllers;

public class KeyPressModel
{
    public byte NumberKey { get; set; }
    public bool LAlt { get; set; }
}

[ApiController]
[Route("[controller]")]
public class KeyboardController : ControllerBase
{
    [DllImport("user32.dll", SetLastError = true)]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    // access via /api/keyboard


    private const uint DW_FLAG_EXTENDEDKEY = 0x0;
    private const uint DW_FLAG_PRESSED = 0x0;
    private const uint DW_FLAG_RELEASED = 0x2;


    private const byte ALT_KEY = 0xA4;
    private const byte LALT_SCAN_CODE = 0x38;
    private const byte ONE_SCAN_CODE = 0x2;

    private const byte ONE_KEY = 0x31;

    [HttpPost]
    public string SendKeyStroke(KeyPressModel model)
    {
        if (model.LAlt)
        {
            keybd_event(ALT_KEY, 0, DW_FLAG_EXTENDEDKEY | DW_FLAG_PRESSED, 0);
        }

        if(model.NumberKey > 9 || model.NumberKey < 0)
        {
            throw new InvalidOperationException();
        }

        byte vKey = (byte)(ONE_KEY + (model.NumberKey - 0b1));
        byte scanCode = (byte)(ONE_SCAN_CODE + (model.NumberKey - 1));

        keybd_event(vKey, 0, DW_FLAG_EXTENDEDKEY | DW_FLAG_PRESSED, 0);

        Thread.Sleep(250);

        keybd_event(vKey, 0, DW_FLAG_EXTENDEDKEY | DW_FLAG_RELEASED, 0);

        if (model.LAlt)
        {
            keybd_event(ALT_KEY, 0, DW_FLAG_EXTENDEDKEY | DW_FLAG_RELEASED, 0);
        }

        return "OK";
    }

    [HttpGet]
    public string SendAlt1()
    {
        Console.WriteLine("Hello world!");

        keybd_event(ALT_KEY, 0, DW_FLAG_PRESSED, 0);
        keybd_event(ONE_KEY, 0, DW_FLAG_PRESSED, 0);
        keybd_event(ONE_KEY, 0, DW_FLAG_RELEASED, 0);
        keybd_event(ALT_KEY, 0, DW_FLAG_RELEASED, 0);

        return "OK";
    }
}

