import { inject, Injectable, signal } from '@angular/core';
import { PartApiServices } from '../api-services/part-api-services';
import { IPart } from '../../interfaces/customize-data.model';

@Injectable({ providedIn: 'root' })
export class PartService {
  // DI
  private readonly _partApiService = inject(PartApiServices);

  // signals
  private _isLoading = signal<boolean>(false);
  private _parts = signal<IPart[]>([]);

  // getters

  get parts() {
    return this._parts.asReadonly();
  }

  get isLoading() {
    return this._isLoading.asReadonly();
  }

  // methods

  updateParts() {
    if (this._isLoading()) return;
    this._isLoading.set(true);

    this._partApiService.getAllParts().subscribe({
      next: (data) => {
        this._parts.set(data);
      },
    });
  }
}
