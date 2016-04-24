#include <atlstr.h>
#include <iomanip>
#include <iostream>
using namespace std;

int main(void)
{
	TCHAR t;
	TCHAR* u;

	//1
	t = _T('X');
	u = _T("aaa");



	cout << t << endl << hex << t << endl;
	cout << u << endl << hex << u << endl;


	system("pause");
}
