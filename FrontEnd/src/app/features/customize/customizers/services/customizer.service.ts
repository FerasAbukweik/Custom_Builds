import { Injectable, signal } from '@angular/core';
import { ICustomizeData } from '../layout/customizer.layout/customizer.model';

@Injectable()
export class CustomizerService {
  private controllerCustomizeData: Record<string, ICustomizeData> = {
    shell: {
      id: 1,
      icon: 'fa-solid fa-shield-halved',
      sections: [
        {
          id: 1,
          title: 'Shell Finish',
          price: 'Standard',
          type: 'color',
          items: [
            { id: 1, name: 'Matte Black', value: '#111722' },
            { id: 2, name: 'Soft White', value: '#cbd5e1' },
            { id: 3, name: 'Cobalt Blue', value: '#135bec' },
            { id: 4, name: 'Platinum', value: '#c0c0c0' },
          ],
        },
        {
          id: 2,
          title: 'Texture',
          price: '+$15.00',
          type: 'image',
          items: [
            {
              id: 5,
              name: 'Carbon',
              value: 'assets/images/keyboard-image.png',
            },
            {
              id: 6,
              name: 'Honeycomb',
              value: 'assets/images/keyboard-image.png',
            },
            {
              id: 7,
              name: 'Digital Camo',
              value: 'assets/images/keyboard-image.png',
            },
          ],
        },
      ],
    },
    Buttons: {
      id: 2,
      icon: 'fa-regular fa-circle-dot',
      sections: [
        {
          id: 3,
          title: 'Button Kit',
          price: 'Standard',
          type: 'color',
          items: [
            { id: 8, name: 'Standard Black', value: '#111' },
            { id: 9, name: 'Crystal Clear', value: '#f8fafc' },
            { id: 10, name: 'Neon Red', value: '#ff1f1f' },
          ],
        },
      ],
    },
    Sticks: {
      id: 3,
      icon: 'fa-solid fa-crosshairs',
      sections: [
        {
          id: 4,
          title: 'Stick Type',
          price: '+$10.00',
          type: 'card',
          items: [
            {
              id: 11,
              name: 'Concave',
              desc: 'Classic grip',
              icon: 'fa-solid fa-circle-dot',
            },
            {
              id: 12,
              name: 'Domed',
              desc: 'Increased range',
              icon: 'fa-solid fa-circle',
            },
          ],
        },
      ],
    },
    Paddles: {
      id: 4,
      icon: 'fa-solid fa-grip-lines',
      sections: [
        {
          id: 5,
          title: 'Rear Paddles',
          price: '+$30.00',
          type: 'card',
          items: [
            {
              id: 13,
              name: 'Standard 2-Pack',
              desc: 'Left/Right mapping',
              icon: 'fa-solid fa-grip-lines',
            },
            {
              id: 14,
              name: 'Ultimate 4-Pack',
              desc: 'Full control',
              icon: 'fa-solid fa-grip-lines-vertical',
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
