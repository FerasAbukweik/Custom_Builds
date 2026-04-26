import { Component, computed, inject, Input, signal } from '@angular/core';
import { CommonModule, KeyValuePipe } from '@angular/common';
import { CustomizerService } from '../../../../services/customizer.service';

@Component({
  selector: 'aside[customizerSideBar]',
  imports: [CommonModule, KeyValuePipe],
  templateUrl: './customizer-sidebar.component.html',
  host: {
    class:
      'w-full lg:w-100 bg-primary/95 backdrop-blur-xl border-l border-dark-blue-gray flex flex-col @container',
  },
})
export class CustomizerSidebarComponent {
  customizerService = inject(CustomizerService);

  get customizeData() {
    return this.customizerService.getCustomizeData;
  }

  get selectedProduct() {
    return this.customizerService.selectedProduct();
  }

  activeTab = signal<keyof typeof this.customizeData>('shell');

  currentTabContent = computed(() => this.customizeData[this.activeTab()]);

  setActiveTab(tabName: string): void {
    this.activeTab.set(tabName);
  }
}
