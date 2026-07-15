// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Client._ADT.VendingMachines.UI;
using Content.Client.UserInterface.Controls;
using Content.Client.VendingMachines.UI;
using Content.Shared.VendingMachines;
using Robust.Client.UserInterface;
using Robust.Shared.Input;
using System.Linq;

namespace Content.Client.VendingMachines
{
    public sealed class VendingMachineBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private FancyVendingMachineMenu? _menu;

        [ViewVariables]
        private List<VendingMachineInventoryEntry> _cachedInventory = new();

        public VendingMachineBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        protected override void Open()
        {
            base.Open();

            _menu = new();
            var component = EntMan.GetComponent<VendingMachineComponent>(Owner);
            var system = EntMan.System<VendingMachineSystem>();
            _cachedInventory = system.GetAllInventory(Owner, component);
            _menu.Title = EntMan.GetComponent<MetaDataComponent>(Owner).EntityName;

            _menu.OnClose += Close;
            _menu.OnItemSelected += OnItemSelected;
            _menu.OnWithdraw += () => SendMessage(new VendingMachineWithdrawMessage());
            _menu.Populate(Owner, _cachedInventory, component.PriceMultiplier, component.Credits);
            SendMessage(new VendingMachineRequestUpdateMessage());

            _menu.OpenCentered();
        }

        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);

            if (state is not VendingMachineUpdateState newState || _menu == null)
                return;

            var system = EntMan.System<VendingMachineSystem>();
            _cachedInventory = system.GetAllInventory(Owner);
            _menu.Populate(Owner, _cachedInventory, newState.PriceMultiplier, newState.Credits);
        }

        private void OnItemSelected(VendingMachineInventoryEntry entry)
        {
            SendMessage(new VendingMachineEjectCountMessage(entry, 1));
        }

        public void Refresh()
        {
            var enabled = EntMan.TryGetComponent(Owner, out VendingMachineComponent? bendy) && !bendy.Ejecting;

            var system = EntMan.System<VendingMachineSystem>();
            _cachedInventory = system.GetAllInventory(Owner);

            _menu?.Populate(Owner, _cachedInventory, bendy?.PriceMultiplier ?? 1, bendy?.Credits ?? 0);
        }

        public void UpdateAmounts()
        {
            var enabled = EntMan.TryGetComponent(Owner, out VendingMachineComponent? bendy) && !bendy.Ejecting;

            var system = EntMan.System<VendingMachineSystem>();
            _cachedInventory = system.GetAllInventory(Owner);
            _menu?.Populate(Owner, _cachedInventory, bendy?.PriceMultiplier ?? 1, bendy?.Credits ?? 0);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;

            if (_menu == null)
                return;

            _menu.OnItemSelected -= OnItemSelected;
            _menu.OnClose -= Close;
            _menu.Dispose();
        }
    }
}
