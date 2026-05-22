import { Component, computed, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomizerService } from '../../../../../customizer.service';

@Component({
  selector: 'aside[customizerSideBar]',
  imports: [CommonModule],
  templateUrl: './customizer-sidebar.component.html',
  host: {
    class:
      'w-full lg:w-100 bg-primary/95 backdrop-blur-xl border-l border-dark-blue-gray flex flex-col @container',
  },
})
export class CustomizerSidebarComponent implements OnInit {
  // injections
  private readonly _customizerService = inject(CustomizerService);

  // signals
  activePartId = this._customizerService.getActivePartId;
  customizeData = this._customizerService.getCustomizeData;
  selectedModifications = this._customizerService.getSelectedModifications;

  // computed
  currentPartSections = computed(
    () => this.customizeData()?.find((part) => part.id === this.activePartId())?.sections ?? [],
  );



  ngOnInit() {
    this._customizerService.updateModifications();
  }

  // methods
  setActivePartId = this._customizerService.setActivePartId;

  addToCart = this._customizerService.addToCart;

  selectModification = this._customizerService.selectModification;
}
