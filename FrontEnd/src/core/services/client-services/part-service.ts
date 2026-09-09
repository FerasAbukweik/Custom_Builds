import { inject, Injectable, signal } from '@angular/core';
import { PartApiServices } from '../api-services/part-api-services';
import { IPart } from '../../interfaces/customize-data.model';
import { SectionApiServices } from '../api-services/section-api-service';
import { ModificationAddDTO } from 'src/core/DTO/modification-add-dto';
import { ModificationApiServices } from '../api-services/modification-api-service';

@Injectable({ providedIn: 'root' })
export class PartService {
  // DI
  private readonly _partApiService = inject(PartApiServices);
  private readonly _sectionApiService = inject(SectionApiServices);
  private readonly _modificationApiService = inject(ModificationApiServices);

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

  addPart(icon: string, name: string) {
    // api call
    this._partApiService.addPart(icon, name).subscribe({
      next: (newPart) => {
        // update the id of the new part
        this._parts.update((curr) => [newPart, ...curr]);
      },
    });
  }

  addSection(partId: string, name: string) {
    // api call
    this._sectionApiService.add(partId, name).subscribe({
      next: (newSection) => {
        this._parts.update((curr) =>
          curr.map((p) => (p.id === partId ? { ...p, sections: [newSection, ...p.sections] } : p)),
        );
      },
    });
  }

  addModification(newModData: ModificationAddDTO) {
    // find the partId of the modification
    let partId = this._parts().find((p) =>
      p.sections.some((s) => s.id === newModData.sectionId),
    )?.id;
    if (!partId) return;

    // call api
    this._modificationApiService.add(newModData).subscribe({
      next: (newModification) => {
        this._parts.update((curr) =>
          curr.map((p) =>
            p.id === partId
              ? {
                  ...p,
                  sections: p.sections.map((s) =>
                    s.id === newModData.sectionId
                      ? { ...s, modifications: [newModification, ...s.modifications] }
                      : s,
                  ),
                }
              : p,
          ),
        );
      },
    });
  }

  removeMod(modId: string) {
    // old data
    const oldParts = this._parts();

    // find the partId of the modification
    let partId = oldParts.find((p) =>
      p.sections.some((s) => s.modifications.some((m) => m.id === modId)),
    )?.id;
    if (!partId) return;

    // optimistic update
    this._parts.update((curr) =>
      curr.map((p) =>
        p.id === partId
          ? {
              ...p,
              sections: p.sections.map((s) =>
                s.modifications.some((m) => m.id === modId)
                  ? { ...s, modifications: s.modifications.filter((m) => m.id !== modId) }
                  : s,
              ),
            }
          : p,
      ),
    );

    // call api
    this._modificationApiService.remove(modId).subscribe({
      error: () => {
        // revert to old data if error occurs
        this._parts.set(oldParts);
      },
    });
  }
}
