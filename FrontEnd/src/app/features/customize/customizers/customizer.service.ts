import { Injectable, signal } from '@angular/core';
import { IPart } from '../../../core/interfaces/customize-data/customize-data.model';

@Injectable()
export class CustomizerService {
  // so we can only choose one modification per section
  // field Id ,  product Id
  private _selectedProduct = signal<Record<string, string>>({});

  selectedProduct = this._selectedProduct.asReadonly();

  selectModification(sectionId: string, productId: string) {
    this._selectedProduct.update((curr) => {
      const isSelected = curr[sectionId] === productId;

      // if already selected remove it
      const newValue = isSelected ? '' : productId;

      return { ...curr, [sectionId]: newValue };
    });
  }
}
