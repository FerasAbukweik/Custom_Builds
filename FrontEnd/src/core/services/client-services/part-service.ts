import { inject, Injectable, signal } from '@angular/core';
import { PartApiServices } from '../api-services/part-api-services';
import { IPart } from '../../interfaces/customize-data.model';

@Injectable({ providedIn: 'root' })
export class PartService {
  // DI
  private readonly partApiService = inject(PartApiServices);

  // signals
  private isLoading = signal<boolean>(false);
  private parts = signal<IPart[]>([]);

  // getters

  get getParts() {
    return this.parts.asReadonly();
  }

  get getIsLoading() {
    return this.isLoading.asReadonly();
  }

  // methods

  updateParts() {
    if (this.isLoading()) return;
    this.isLoading.set(true);

    this.partApiService.getAllParts().subscribe({
      next: (data) => {
        this.parts.set(data);
      },
    });
  }
}
