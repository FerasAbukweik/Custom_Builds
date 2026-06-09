import { Component } from '@angular/core';

@Component({
  selector: 'app-inventory-management',
  imports: [],
  templateUrl: './inventory-management.component.html',
})
export class InventoryManagementComponent {
navItems = [
    { icon: 'dashboard', name: 'Dashboard', link: '#', active: false },
    { icon: 'shopping_bag', name: 'Orders', link: '#', active: false },
    { icon: 'inventory_2', name: 'Inventory', link: '#', active: true },
    { icon: 'group', name: 'Customers', link: '#', active: false },
    { icon: 'palette', name: 'Design Library', link: '#', active: false }
  ];

  products = [
    {
      name: 'K65 Zenith Mechanical',
      sku: 'AC-K65-ZEN-01',
      category: 'Mechanical Keyboards',
      image: 'https://lh3.googleusercontent.com/aida-public/AB6AXuC9PxbMSIlUrx4j4BiqH8Mx3B384IdGCZUZoRutg7BtvsCGCbIHKRh0PHkllV0JQGgvW9BxyxpnyDnOE6_OIZFVZhRU8aCwdfPBoXWq0DfHsqoyp9twnH7ggmbq8_7xCLYkNvOVs5pwdEmHUzn1ZgGgb5J1g_l3qougo7unr9Ksc2WQVH1PdQhtOzhQ6CDK6jRX2RfSG2-Cpy9pD5M0jsnbcg0VnGQ55e6CBhQtgR8pQO1me78iF54rjCoMQcu2XblG56TVTp5eHE4p',
      stock: 124,
      status: 'low',
      statusText: 'Low stock warning'
    },
    {
      name: 'AeroPro X Wireless',
      sku: 'AC-CTL-APX-02',
      category: 'Gaming Controllers',
      image: 'https://lh3.googleusercontent.com/aida-public/AB6AXuDP7YhIZb1CMPfbdHjTWd8F6sy4mGmjTCkisga78WZniawK6Mie9ThnAGJHIPxwB7WcROECOoYz02bVSta0NsEBpvPukcsQWlsrW1r2sHXpCihSU8rr6Gn32V2m0XuHaF8EhdBqtbnzsNoTA8zsn4JMvBi-hNzh_jc4N7h6KnGGOnbXb_7TOsALUWqS4f-T9RVpSjYR-jIHhxd-UOj29IDyD6XmtQ03VH8y_rXCXHrJH2QfB_RQ7GaDIBi0eeI1rJPKmSsVJDzpN7kD',
      stock: 850,
      status: 'healthy',
      statusText: 'Healthy stock'
    }
  ];

  parts = [
    { icon: 'videogame_asset', name: 'PS5 Shell', active: true },
    { icon: 'keyboard', name: '65% PCB Base', active: false },
    { icon: 'cable', name: 'Aviator Cables', active: false }
  ];

  sections = [
    { icon: 'layers', name: 'Faceplate', active: true },
    { icon: 'touchpad_mouse', name: 'Touchpad', active: false },
    { icon: 'switch_access_shortcut', name: 'Triggers (L2/R2)', active: false }
  ];

  mods = [
    { name: 'Matte Black', price: '+$15.00 Premium', colorClass: 'bg-[#000] border-dark-gray' },
    { name: 'Iridescent Blue', price: '+$25.00 Premium', colorClass: 'bg-secondary border-off-white/20 shadow-[0_0_8px_rgba(19,91,236,0.4)]' },
    { name: 'Carbon Fiber', price: '+$35.00 Premium', colorClass: 'bg-dark-gray border-dark-blue-gray' }
  ];

  logs = [
    { 
      time: '14:22:05', 
      entity: 'PS5 Shell > Triggers', 
      action: "Price modified for 'Digital Click'", 
      adminInitials: 'AV',
      adminName: 'Alex Vane',
      adminBg: 'bg-secondary/20 text-secondary',
      status: 'Live',
      statusClass: 'bg-secondary/10 text-secondary border-secondary/20'
    },
    { 
      time: '12:10:48', 
      entity: 'K65 Zenith', 
      action: 'Stock decremented (Order #8821)', 
      adminInitials: 'SYS',
      adminName: 'Automation Bot',
      adminBg: 'bg-dark-gray text-off-white',
      status: 'Logged',
      statusClass: 'bg-dark-deep-blue text-soft-gray border-dark-blue-gray'
    }
  ];
}
