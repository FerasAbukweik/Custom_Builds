import { Injectable, signal } from '@angular/core';
import { OrderDetailsDialogComponent } from './order-details-dialog.component';

@Injectable({ providedIn: 'root' })
export class OrderDetailsDialogService {
  // private
  private _component?: OrderDetailsDialogComponent;
  private _selectedOrderId = signal<string>('');

  // getters

  get selectedOrderId() {
    return this._selectedOrderId.asReadonly();
  }

  // methods

  register(component: OrderDetailsDialogComponent) {
    this._component = component;
  }

  openDialog(orderId: string) {
    if (!this._component) return; // show error message

    this._selectedOrderId.set(orderId);
    this._component.openModal();
  }

  closeDialog() {
    if (!this._component) return; // show error message

    this._component.closeModal();
    this._selectedOrderId.set('');
  }
}
