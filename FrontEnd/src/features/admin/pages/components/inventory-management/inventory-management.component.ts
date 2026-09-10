import { Component, computed, ElementRef, inject, OnInit, signal, viewChild } from '@angular/core';
import { ProductService } from 'src/core/services/client-services/product-service';
import { LoadingComponent } from 'src/shared/components/loading/loading.component';
import { IsVisableDirective } from 'src/shared/directives/is-visable.directive';
import { ProductDialogComponent } from 'src/shared/components/product-dialog/product-dialog.component';
import { ProductDialogService } from 'src/shared/components/product-dialog/product-dialog.service';
import { PartService } from 'src/core/services/client-services/part-service';
import { ModificationAddDTO } from 'src/core/DTO/modification-add-dto';

enum Adding {
  Part,
  Section,
  Modification,
  none,
}

@Component({
  selector: 'app-inventory-management',
  imports: [LoadingComponent, IsVisableDirective, ProductDialogComponent],
  providers: [ProductDialogService],
  templateUrl: './inventory-management.component.html',
})
export class InventoryManagementComponent implements OnInit {
  // DI
  protected readonly productsService = inject(ProductService);
  protected readonly productDialogService = inject(ProductDialogService);
  protected readonly partService = inject(PartService);

  // signals
  protected selectedPartId = signal<string>('');
  protected selectedSectionId = signal<string>('');
  protected isAdding = signal<Adding>(Adding.none);

  // view child
  private modificationImageInput =
    viewChild<ElementRef<HTMLInputElement>>('modificationImageInput');

  // computed
  protected sections = computed(
    () => this.partService.parts().find((p) => p.id === this.selectedPartId())?.sections ?? [],
  );
  protected mods = computed(
    () => this.sections().find((s) => s.id === this.selectedSectionId())?.modifications ?? [],
  );

  // methods

  ngOnInit() {
    this.partService.updateParts();
  }

  toggleIsAdding(newAdding: Adding) {
    this.isAdding.update((curr) => (curr === Adding.none ? newAdding : Adding.none));
  }

  stopAdding() {
    this.isAdding.set(Adding.none);
  }

  handleAddPart(icon: string, name: string) {
    if (!icon || !name) return;

    this.partService.addPart(icon, name);
    this.stopAdding();
  }

  handleAddSection(partId: string, name: string) {
    if (!partId || !name) return;

    this.partService.addSection(partId, name);
    this.stopAdding();
  }

  handleAddModification(sectionId: string, name: string, price: string) {
    if (!sectionId || !name || !price) return;

    const files = this.modificationImageInput()?.nativeElement.files;
    if (!files || files.length === 0) return;

    const newModData: ModificationAddDTO = {
      sectionId: sectionId,
      image: files[0],
      name: name,
      price: parseFloat(price),
    };

    this.partService.addModification(newModData);
    this.modificationImageInput()!.nativeElement.value = '';
    this.stopAdding();
  }
}
