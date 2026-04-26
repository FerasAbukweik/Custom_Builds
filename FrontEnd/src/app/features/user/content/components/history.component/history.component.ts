import { Component, signal } from '@angular/core';
import { IOrderHistory, IStatsData } from './history.component.model';
import { CommonModule } from '@angular/common';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';

@Component({
  selector: 'app-history',
  imports: [CommonModule, UserContentWrapper],
  templateUrl: './history.component.html',
})
export class HistoryComponent {
statsData: IStatsData[] = [
    { label: "Total Spent", value: "$1,248.50" },
    { label: "Total Orders", value: "12" },
    { label: "Active Builds", value: "2", highlight: true },
    { label: "Loyalty Points", value: "840", badge: "GOLD" },
  ];

  orderHistory: IOrderHistory[] = [
    { id: "#CP-8291", date: "Sept 12, 2023", item: "'Neon-Night' 65% Keyboard", specs: "Custom PCB, Cherry MX Blue", status: "Delivered", price: "$249.00", statusType: "ok" },
    { id: "#CP-7742", date: "Aug 28, 2023", item: "Pro-Grip Wireless Mouse", specs: "Carbon Fiber Shell", status: "Delivered", price: "$159.99", statusType: "ok" },
    { id: "#CP-7105", date: "July 04, 2023", item: "Elite Series Controller", specs: "Trigger Stops, Paddle Map", status: "Refunded", price: "$189.00", statusType: "error" },
  ];

  pages = [1, 2, 3];
  currentPage = signal<number>(1);
}
