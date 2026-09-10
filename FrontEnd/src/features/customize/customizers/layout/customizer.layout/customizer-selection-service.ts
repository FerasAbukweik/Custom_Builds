import { Injectable, computed, inject, signal } from '@angular/core';
import { PartService } from 'src/core/services/client-services/part-service';

@Injectable()
export class CustomizerSelectionService {
  private readonly partServices = inject(PartService);

  // State
  readonly selectedModifications = signal<Record<string, string>>({});

  // Add this inside your CustomizerSelectionService class
  readonly selectedModificationImages = computed<string[]>(() => {
    const idsSet = new Set(Object.values(this.selectedModifications()));

    return this.partServices
      .parts()
      .flatMap((p) => p.sections)
      .flatMap((s) => s.modifications)
      .filter((m) => idsSet.has(m.id))
      .map((m) => m.image);
  });

  // Computed Total Price
  readonly totalPrice = computed<number>(() => {
    const idsSet = new Set(Object.values(this.selectedModifications()));

    return 50 + this.partServices
      .parts()
      .reduce(
        (sum, p) =>
          sum +
          p.sections.reduce(
            (sum, s) =>
              sum +
              s.modifications.filter((m) => idsSet.has(m.id)).reduce((sum, m) => sum + m.price, 0),
            0,
          ),
        0,
      );
  });

  // Methods
  selectModification(sectionId: string, modificationId: string) {
    this.selectedModifications.update((curr) => {
      // If the clicked modification is already selected for this section, remove it
      if (curr[sectionId] && curr[sectionId] === modificationId) {
        const updated = { ...curr };
        delete updated[sectionId];
        return updated;
      }

      // Otherwise, select the new modification
      return { ...curr, [sectionId]: modificationId };
    });
  }

  clearSelections() {
    this.selectedModifications.set({});
  }
}
