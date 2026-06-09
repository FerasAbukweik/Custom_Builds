import { Component } from '@angular/core';

@Component({
  selector: 'app-saved-build-view',
  imports: [],
  templateUrl: './saved-build-view.component.html',
  host: {
    class:
      '@container/savedContainer group w-full flex-1 flex flex-col min-[775px]:max-w-[580px] min-[630px]:min-w-[340px] overflow-hidden rounded-2xl bg-dark-deep-blue border border-dark-gray/30',
  },
})
export class SavedBuildViewComponent {}
