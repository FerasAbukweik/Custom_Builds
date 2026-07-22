import { Component } from '@angular/core';
import { StateDataComponent } from '../../../../../../app/shared/components/stateData/state-data.component';
import { stateCardData } from '../../../../../../core/interceptors/state-card-data';
import { OrdersTableComponent } from '../../../../../../app/shared/components/orders-table/orders-table.component';

@Component({
  selector: 'app-orders-management.component',
  imports: [StateDataComponent],
  templateUrl: './orders-management.component.html',
  host: {
    class: 'bg-primary text-off-white min-h-screen font-display w-full flex',
  },
})
export class OrdersManagementComponent {
  public navItems = [
    { icon: 'dashboard', label: 'Dashboard', active: false },
    { icon: 'shopping_bag', label: 'Orders', active: true },
    { icon: 'inventory_2', label: 'Inventory', active: false },
    { icon: 'group', label: 'Customers', active: false },
    { icon: 'palette', label: 'Design Library', active: false },
  ];

  public stats: stateCardData = [
    {
      name: 'Active Orders',
      value: '1,284',
    },
    {
      name: 'In Assembly',
      value: '432',
    },
    {
      name: 'Testing Phase',
      value: '189',
    },
    {
      name: 'Shipped (24h)',
      value: '612',
    },
  ];

  public orders = [
    {
      id: '#CP-8842',
      initials: 'JD',
      name: 'Julianne de Luca',
      avatarStyle: 'bg-dark-deep-blue text-secondary border-secondary',
      productIcon: 'keyboard',
      product: 'Mechanical Keyboard',
      status: 'Assembling',
      statusStyle: 'bg-dark-deep-blue text-secondary border border-dark-blue-gray',
      date: 'Oct 24, 2023',
    },
    {
      id: '#CP-8843',
      initials: 'MK',
      name: 'Marcus Kane',
      avatarStyle: 'bg-dark-blue-gray text-off-white border-soft-gray',
      productIcon: 'sports_esports',
      product: 'Pro Controller',
      status: 'Testing',
      statusStyle: 'bg-dark-blue-gray text-off-white border border-soft-gray',
      date: 'Oct 24, 2023',
    },
    {
      id: '#CP-8844',
      initials: 'SL',
      name: 'Sarah Lindholm',
      avatarStyle: 'bg-dark-deep-blue text-ok border-ok',
      productIcon: 'keyboard',
      product: 'Keycap Set',
      status: 'Ready to Ship',
      statusStyle: 'bg-dark-deep-blue text-ok border border-ok',
      date: 'Oct 23, 2023',
    },
    {
      id: '#CP-8845',
      initials: 'TV',
      name: 'Tariq Varma',
      avatarStyle: 'bg-dark-deep-blue text-secondary border-secondary',
      productIcon: 'keyboard',
      product: 'Mechanical Keyboard',
      status: 'Assembling',
      statusStyle: 'bg-dark-deep-blue text-secondary border border-dark-blue-gray',
      date: 'Oct 23, 2023',
    },
  ];

  public chartBars = [
    { height: '60%', color: 'bg-dark-blue-gray hover:bg-secondary', title: 'Mon: 60%' },
    { height: '75%', color: 'bg-dark-blue-gray hover:bg-secondary', title: 'Tue: 75%' },
    { height: '90%', color: 'bg-secondary hover:bg-light-steel-blue', title: 'Wed: 90%' },
    { height: '65%', color: 'bg-dark-blue-gray hover:bg-secondary', title: 'Thu: 65%' },
    { height: '82%', color: 'bg-light-steel-blue hover:bg-secondary', title: 'Fri: 82%' },
    { height: '55%', color: 'bg-dark-blue-gray hover:bg-secondary', title: 'Sat: 55%' },
    { height: '95%', color: 'bg-secondary hover:bg-off-white', title: 'Sun: 95%' },
  ];
}
