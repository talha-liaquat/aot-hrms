import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JoingroupComponent } from './joingroup.component';

describe('JoingroupComponent', () => {
  let component: JoingroupComponent;
  let fixture: ComponentFixture<JoingroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JoingroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JoingroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
