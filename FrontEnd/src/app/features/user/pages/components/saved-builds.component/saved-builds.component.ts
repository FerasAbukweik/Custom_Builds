import { Component } from '@angular/core';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';
import { SavedBuildViewComponent } from './components/saved-build-view.component/saved-build-view.component';

@Component({
  selector: 'app-saved-builds',
  imports: [UserContentWrapper, SavedBuildViewComponent],
  templateUrl: './saved-builds.component.html',
})
export class SavedBuildsComponent {
  builds = [1, 2, 3, 4, 5, 6];
}
