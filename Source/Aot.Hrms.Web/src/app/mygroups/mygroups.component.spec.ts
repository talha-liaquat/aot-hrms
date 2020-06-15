import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MygroupsComponent } from './mygroups.component';

describe('MygroupsComponent', () => {
  let component: MygroupsComponent;
  let fixture: ComponentFixture<MygroupsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MygroupsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MygroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
