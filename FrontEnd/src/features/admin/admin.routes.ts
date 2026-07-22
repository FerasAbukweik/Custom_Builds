import { Routes } from "@angular/router";

export const routes: Routes = [
    {
        path: 'dashboard',
        loadComponent: () => import('./pages/components/dashboard/dashboard.component').then(x => x.DashboardComponent)
    },
    {
        path: 'orders-management',
        loadComponent: () => import('./pages/components/orders-management/orders-management.component/orders-management.component').then(x => x.OrdersManagementComponent)
    },
    {
        path: 'inventory-management',
        loadComponent: () => import('./pages/components/inventory-management/inventory-management.component').then(x => x.InventoryManagementComponent)
    },
    {
        path: 'support-chat',
        loadComponent: () => import('./pages/components/support-chat/support-chat.component').then(x => x.SupportChatComponent)
    },
    {
        path: '**',
        redirectTo: 'dashboard'
    }
]