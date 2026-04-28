import { Injectable, signal } from '@angular/core';
import { ISection } from '../../../../core/interfaces/customize-data/customize-data.model';

@Injectable()
export class CustomizerService {
  private controllerCustomizeData: Record<string, ISection> = {
    shell: {
      id: 1,
      icon: 'fa-solid fa-shield-halved',
      fields: [
        {
          id: 1,
          title: 'Shell Finish',
          type: 'color',
          items: [
            { id: 1, name: 'Matte Black', value: '#111722', price: 15 },
            { id: 2, name: 'Soft White', value: '#cbd5e1', price: 15 },
            { id: 3, name: 'Cobalt Blue', value: '#135bec', price: 15 },
            { id: 4, name: 'Platinum', value: '#c0c0c0', price: 15 },
          ],
        },
        {
          id: 2,
          title: 'Texture',
          type: 'image',
          items: [
            {
              id: 5,
              name: 'Carbon',
              value: 'assets/images/keyboard-image.png',
              price: 20,
            },
            {
              id: 6,
              name: 'Honeycomb',
              value: 'assets/images/keyboard-image.png',
              price: 20,
            },
            {
              id: 7,
              name: 'Digital Camo',
              value: 'assets/images/keyboard-image.png',
              price: 20,
            },
          ],
        },
      ],
    },
    Buttons: {
      id: 2,
      icon: 'fa-regular fa-circle-dot',
      fields: [
        {
          id: 3,
          title: 'Button Kit',
          type: 'color',
          items: [
            { id: 8, name: 'Standard Black', value: '#111', price: 25 },
            { id: 9, name: 'Crystal Clear', value: '#f8fafc', price: 25 },
            { id: 10, name: 'Neon Red', value: '#ff1f1f', price: 25 },
          ],
        },
      ],
    },
    Sticks: {
      id: 3,
      icon: 'fa-solid fa-crosshairs',
      fields: [
        {
          id: 4,
          title: 'Stick Type',
          type: 'card',
          items: [
            {
              id: 11,
              name: 'Concave',
              desc: 'Classic grip',
              icon: 'fa-solid fa-circle-dot',
              price: 25,
            },
            {
              id: 12,
              name: 'Domed',
              desc: 'Increased range',
              icon: 'fa-solid fa-circle',
              price: 25,
            },
          ],
        },
      ],
    },
    Paddles: {
      id: 4,
      icon: 'fa-solid fa-grip-lines',
      fields: [
        {
          id: 5,
          title: 'Rear Paddles',
          type: 'card',
          items: [
            {
              id: 13,
              name: 'Standard 2-Pack',
              desc: 'Left/Right mapping',
              icon: 'fa-solid fa-grip-lines',
              price: 25,
            },
            {
              id: 14,
              name: 'Ultimate 4-Pack',
              desc: 'Full control',
              icon: 'fa-solid fa-grip-lines-vertical',
              price: 25,
            },
          ],
        },
      ],
    },
  };

  get getCustomizeData() {
    return { ...this.controllerCustomizeData };
  }

  private _selectedProduct = signal<Record<number, number>>({});

  selectedProduct = this._selectedProduct.asReadonly();

  selectProduct(sectionId: number, productId: number) {
    this._selectedProduct.update((prev) => ({ ...prev, [sectionId]: productId }));
  }

  removeProduct(sectionId: number) {
    this._selectedProduct.update((prev) => ({ ...prev, [sectionId]: 0 }));
  }

  toggleProduct(sectionId: number, productId: number) {
    this._selectedProduct.update((prev) => ({
      ...prev,
      [sectionId]: prev[sectionId] === productId ? 0 : productId,
    }));
  }
}
