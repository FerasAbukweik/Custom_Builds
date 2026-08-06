import { Component, DestroyRef, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { MessagesService } from '../../../../../core/services/client-services/messages-service';
import { LiveChattingComponent } from '../../../../../shared/components/live-chatting/live-chatting.component';
import { MessagesSignalRService } from '../../../../../core/services/client-services/messaegs-signalR-service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

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
export class SupportComponent implements OnInit, OnDestroy {
  // injections
  protected readonly messagesService = inject(MessagesService);
  protected readonly messagesSignalRService = inject(MessagesSignalRService);
  private readonly _destroyRef = inject(DestroyRef);

  //signals
  protected isTyping = signal<boolean>(false);

  // fields

  // public
  quickActions = quickActions;

  // methods

  // init
  async ngOnInit() {
    this.messagesSignalRService.startConnection();

    this._handleStoppedTyping();
    this._handleTyping();
  }

  ngOnDestroy() {
    this.messagesSignalRService.stopConnection();
  }

  // handle user is typing
  private _handleTyping = () => {
    this.messagesSignalRService.someoneIsTyping$
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe({
        next: () => {
          this.isTyping.set(true);
        },
      });
  };

  // handle stpped typing
  private _handleStoppedTyping = () => {
    this.messagesSignalRService.noOneIsTyping$
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe({
        next: () => {
          this.isTyping.set(false);
        },
      });
  };

  // manage is typing
  handleTyping = (isTyping: boolean) => {
    if (isTyping) this.messagesSignalRService.notifyStoppedTyping();
    else this.messagesSignalRService.notifyTyping();
  };

  // handel send message
  handleSendMessage = (content: string) => {
    this.handleTyping(false);
    this.messagesSignalRService.sendMessage(content);
  };
}
