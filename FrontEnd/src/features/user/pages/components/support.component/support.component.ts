import { Component, inject, OnInit } from '@angular/core';
import { SupportService } from './support.service';
import { LiveChattingComponent } from '../../../../../shared/components/live-chatting/live-chatting.component';

const quickActions = [
  'Where is my order?',
  'Customization help',
  'Payment issue',
  'Firmware update',
  'Warranty claim',
];

@Component({
  selector: 'app-support',
  imports: [LiveChattingComponent],
  templateUrl: './support.component.html',
  host: {
    class: 'flex flex-col h-full bg-dark-deep-blue overflow-hidden',
  },
})
export class SupportComponent implements OnInit {
  // injections
  private readonly _supportService = inject(SupportService);

  // signals
  messages = this._supportService.getMessages;
  isLoading = this._supportService.getIsLoading;
  isTyping = this._supportService.getIsTyping;
  isSignalRConnected = this._supportService.getIsSignalRConnected;

  // fields

  // public
  quickActions = quickActions;

  // methods

  // init
  async ngOnInit() {
    this._supportService.init();
  }

  // handle send message
  handleSendMessage = this._supportService.handleSendMessage;

  // handle is typing
  handleIsTyping = this._supportService.handleIsTyping;

  // lazy Load Messages
  lazyLoadMessages = this._supportService.lazyGetMessages;
}
