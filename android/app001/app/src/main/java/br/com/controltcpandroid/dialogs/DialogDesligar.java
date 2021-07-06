package br.com.controltcpandroid.dialogs;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;

/**
 * https://stackoverflow.com/questions/2115758/how-do-i-display-an-alert-dialog-on-android
 */
public class DialogDesligar {

    public void showDialogDesligar(String mensagem, Runnable mtSim, Runnable mtNao, Context context){
        AlertDialog.Builder builder1 = new AlertDialog.Builder(context);
        builder1.setMessage(mensagem);
        builder1.setCancelable(true);

        builder1.setPositiveButton(
                "Sim",
                new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        if (mtSim!=null) {
                            mtSim.run();
                        }
                        dialog.cancel();
                    }
                });

        builder1.setNegativeButton(
                "Não",
                new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        if (mtNao!=null) {
                            mtNao.run();
                        }
                        dialog.cancel();
                    }
                });

        AlertDialog alert11 = builder1.create();
        alert11.show();
    }

}