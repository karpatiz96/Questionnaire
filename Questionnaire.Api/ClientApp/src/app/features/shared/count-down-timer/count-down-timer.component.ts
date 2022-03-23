import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-count-down-timer',
  templateUrl: './count-down-timer.component.html',
  styleUrls: ['./count-down-timer.component.css']
})
export class CountDownTimerComponent implements OnInit, OnDestroy {
  private countDownSubscription: Subscription;
  // minutes
  @Input() time: number;
  @Output() notification: EventEmitter<string> = new EventEmitter<string>();
  start: Date = new Date();
  finish: Date;

  timeRemaning: number;

  milliSecondsInSecond = 1000;
  secondsInMinute = 60;
  minutesInHour = 60;
  hoursInDay = 24;

  secondsToFinish: number;
  minutesToFinish: number;
  hoursToFinish: number;
  daysToFinish: number;

  constructor() { }

  ngOnInit() {
    const finishTime = this.start.getTime() + this.time * this.secondsInMinute * this.milliSecondsInSecond;
    this.finish = new Date(finishTime);
    this.countDownSubscription = interval(1000).subscribe(() => {
      this.getTimeRemaning();
      if (this.timeRemaning < 0) {
        this.notification.emit('Finished');
      }
    });
  }

  private getTimeRemaning() {
    this.timeRemaning = this.finish.getTime() - new Date().getTime();
    this.setTimeDisplay(this.timeRemaning);
  }

  private setTimeDisplay(timeRemaning: number) {
    this.secondsToFinish = Math.floor((timeRemaning / this.milliSecondsInSecond) % this.secondsInMinute);
    this.minutesToFinish = Math.floor((timeRemaning / (this.milliSecondsInSecond * this.secondsInMinute)) % this.minutesInHour);
    this.hoursToFinish = Math.floor((timeRemaning /
      (this.milliSecondsInSecond * this.secondsInMinute * this.minutesInHour)) % this.hoursInDay);
    this.daysToFinish = Math.floor((timeRemaning /
      (this.milliSecondsInSecond * this.secondsInMinute * this.minutesInHour * this.hoursInDay)));
  }

  ngOnDestroy(): void {
    this.countDownSubscription.unsubscribe();
  }

}
