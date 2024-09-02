#include <Windows.h>
#include <cstdio>

#define _Assembly ".\\CeLeRy\\CeLeRy.dll"
#define _Class "CeLeRy.Entry"
#define _Method "Main"

using namespace System;
using namespace System::Reflection;

void LoadCeLeRy() 
{
    Assembly^ assembly = Assembly::LoadFrom(_Assembly);
    Type^ type = assembly->GetType(_Class);
    MethodInfo^ mi = type->GetMethod(_Method);
    mi->Invoke(nullptr, nullptr);
}

DWORD WINAPI Main(LPVOID lpParam)
{
    AllocConsole();
    FILE* fDummy;
    freopen_s(&fDummy, "CONIN$", "r", stdin);
    freopen_s(&fDummy, "CONOUT$", "w", stderr);
    freopen_s(&fDummy, "CONOUT$", "w", stdout);

    LoadCeLeRy();

    // Keep Thread Running
    while ((true)) {} 
    return 0;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    if (ul_reason_for_call == DLL_PROCESS_ATTACH) {
        DisableThreadLibraryCalls(hModule);
        CreateThread(0, 0, (LPTHREAD_START_ROUTINE)Main, hModule, 0, 0);
    }
    return TRUE;
}

