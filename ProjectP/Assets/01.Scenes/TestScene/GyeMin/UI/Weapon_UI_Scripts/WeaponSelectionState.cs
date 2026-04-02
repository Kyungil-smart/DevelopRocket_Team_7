using UnityEngine;

public static class WeaponSelectionState
{
    public static WeaponCatalogEntry SelectedEntry { get; private set; }

    public static bool HasSelection => SelectedEntry != null;

    public static void SetSelection(WeaponCatalogEntry entry)
    {
        SelectedEntry = entry;
    }

    public static void Clear()
    {
        SelectedEntry = null;
    }
}